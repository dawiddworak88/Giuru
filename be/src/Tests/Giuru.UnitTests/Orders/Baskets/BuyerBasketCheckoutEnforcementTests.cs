using Buyer.Web.Areas.Orders.ApiControllers;
using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Orders.Repositories.UserApprovals;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Basket;
using Buyer.Web.Shared.Definitions.Middlewares;
using Buyer.Web.Shared.DomainModels.Clients;
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.Repositories.Identity;
using Buyer.Web.Shared.Services.Baskets;
using Foundation.Localization;
using Foundation.Pricing.DiscountCodes;
using Giuru.UnitTests.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using BuyerBasket = Buyer.Web.Areas.Orders.DomainModels.Basket;

namespace Giuru.UnitTests.Orders.Baskets
{
    // Buyer checkout: with enforcement on, the identities the code is checked for are the ones the order is placed for -
    // the client comes from the authenticated principal and the basket from the cookie, never from the request body.
    public class BuyerBasketCheckoutEnforcementTests
    {
        private const string Token = "token";
        private static readonly Guid CookieBasketId = Guid.NewGuid();
        private static readonly Guid BodyBasketId = Guid.NewGuid();
        private static readonly Guid ClientId = Guid.NewGuid();
        private static readonly Guid BasketVersion = Guid.NewGuid();

        private sealed class Fixture
        {
            public IBasketRepository BasketRepository { get; } = Substitute.For<IBasketRepository>();
            public IBasketService BasketService { get; } = Substitute.For<IBasketService>();
            public IDiscountCodeValidator Validator { get; } = Substitute.For<IDiscountCodeValidator>();
            public BasketCheckoutApiController Controller { get; }

            public Fixture(BuyerBasket storedBasket, bool grulaConfigured = true, bool enforcementEnabled = true, Guid? principalClientId = null, bool withClient = true)
            {
                BasketRepository.GetBasketById(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>()).Returns(Task.FromResult<BuyerBasket>(null));
                BasketRepository.GetBasketById(Arg.Any<string>(), Arg.Any<string>(), CookieBasketId).Returns(Task.FromResult(storedBasket));

                var options = Options.Create(new AppSettings
                {
                    GrulaAccessToken = grulaConfigured ? "grula-token" : null,
                    GrulaEnvironmentId = Guid.NewGuid().ToString(),
                    DiscountCodeEnforcementEnabled = enforcementEnabled
                });

                var clientAddresses = Substitute.For<IClientAddressesRepository>();
                clientAddresses.GetAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<Guid>>())
                    .Returns(Task.FromResult<IEnumerable<ClientAddress>>(Array.Empty<ClientAddress>()));

                Controller = new BasketCheckoutApiController(
                    BasketRepository,
                    BasketService,
                    clientAddresses,
                    TestLocalizer.Create<OrderResources>(),
                    Substitute.For<IUserApprovalsRepository>(),
                    Substitute.For<IIdentityRepository>(),
                    TestLocalizer.Create<ClientResources>(),
                    options,
                    Validator)
                {
                    ControllerContext = new ControllerContext { HttpContext = CreateHttpContext(withClient ? principalClientId ?? ClientId : null) }
                };
            }

            public Task<IActionResult> CheckoutAsync(Guid? bodyClientId = null, bool hasCustomOrder = false)
            {
                return Controller.Checkout(new CheckoutBasketRequestModel
                {
                    BasketId = BodyBasketId,
                    ClientId = bodyClientId,
                    HasCustomOrder = hasCustomOrder
                });
            }

            public Task AssertNoCheckoutAsync()
            {
                return BasketRepository.DidNotReceiveWithAnyArgs().CheckoutBasketAsync(
                    default, default, default, default, default, default, default, default, default, default, default, default, default, default);
            }

            public Task AssertCheckedOutAsync(Guid? clientId, Guid? basketId, Guid? expectedVersion)
            {
                return BasketRepository.Received(1).CheckoutBasketAsync(
                    Token, Arg.Any<string>(), clientId, Arg.Any<string>(), Arg.Any<string>(), basketId,
                    Arg.Any<ClientAddress>(), Arg.Any<ClientAddress>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>(),
                    Arg.Any<IEnumerable<Guid>>(), Arg.Any<Guid?>(), expectedVersion);
            }
        }

        private static BuyerBasket Basket(string discountCode = null, Guid? discountCodeClientId = null, Guid? version = null)
        {
            return new BuyerBasket
            {
                Id = CookieBasketId,
                DiscountCode = discountCode,
                DiscountCodeClientId = discountCodeClientId,
                BasketVersion = version,
                Items = new[] { new BasketItem { ProductId = Guid.NewGuid(), ProductSku = "SKU", Quantity = 1 } }
            };
        }

        private static void ValidatorAnswers(Fixture fixture, DiscountCodeValidation validation)
        {
            fixture.Validator.ValidateAsync(Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(validation));
        }

        private static int StatusOf(IActionResult result) => ((ObjectResult)result).StatusCode.Value;

        private static string MessageOf(IActionResult result)
        {
            var value = ((ObjectResult)result).Value;

            return (string)value.GetType().GetProperty("Message").GetValue(value);
        }

        [Fact]
        public async Task Checkout_WithEnforcementOff_KeepsTheLegacyIdentitiesAndValidatesNothing()
        {
            var fixture = new Fixture(Basket("SUMMER25"), enforcementEnabled: false);
            var bodyClientId = Guid.NewGuid();

            var result = await fixture.CheckoutAsync(bodyClientId);

            Assert.Equal((int)HttpStatusCode.Accepted, StatusOf(result));
            await fixture.Validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
            await fixture.BasketService.Received(1).ValidateStockOutletQuantitiesAsync(BodyBasketId, Token, Arg.Any<string>());
            await fixture.AssertCheckedOutAsync(bodyClientId, CookieBasketId, null);
        }

        [Fact]
        public async Task Checkout_WhenGrulaIsNotConfigured_KeepsTheLegacyIdentitiesAndValidatesNothing()
        {
            var fixture = new Fixture(Basket("SUMMER25"), grulaConfigured: false);
            var bodyClientId = Guid.NewGuid();

            var result = await fixture.CheckoutAsync(bodyClientId);

            Assert.Equal((int)HttpStatusCode.Accepted, StatusOf(result));
            await fixture.Validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
            await fixture.AssertCheckedOutAsync(bodyClientId, CookieBasketId, null);
        }

        [Fact]
        public async Task Checkout_WithEnforcementOn_UsesTheCookieBasketForTheReadTheStockCheckAndTheOrder()
        {
            var fixture = new Fixture(Basket(version: BasketVersion));

            var result = await fixture.CheckoutAsync();

            Assert.Equal((int)HttpStatusCode.Accepted, StatusOf(result));
            await fixture.BasketRepository.Received(1).GetBasketById(Token, Arg.Any<string>(), CookieBasketId);
            await fixture.BasketRepository.DidNotReceive().GetBasketById(Arg.Any<string>(), Arg.Any<string>(), BodyBasketId);
            await fixture.BasketService.Received(1).ValidateStockOutletQuantitiesAsync(CookieBasketId, Token, Arg.Any<string>());
            await fixture.BasketService.DidNotReceive().ValidateStockOutletQuantitiesAsync(BodyBasketId, Arg.Any<string>(), Arg.Any<string>());
            await fixture.AssertCheckedOutAsync(ClientId, CookieBasketId, BasketVersion);
        }

        [Fact]
        public async Task Checkout_WithEnforcementOn_RejectsABodyClientThatDiffersFromThePrincipal()
        {
            var fixture = new Fixture(Basket("SUMMER25", ClientId, BasketVersion));

            var result = await fixture.CheckoutAsync(bodyClientId: Guid.NewGuid());

            Assert.Equal((int)HttpStatusCode.BadRequest, StatusOf(result));
            Assert.Equal("ClientNotFound", MessageOf(result));
            await fixture.Validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
            await fixture.AssertNoCheckoutAsync();
        }

        [Fact]
        public async Task Checkout_WithEnforcementOn_AcceptsABodyClientThatMatchesThePrincipal()
        {
            var fixture = new Fixture(Basket(version: BasketVersion));

            var result = await fixture.CheckoutAsync(bodyClientId: ClientId);

            Assert.Equal((int)HttpStatusCode.Accepted, StatusOf(result));
            await fixture.AssertCheckedOutAsync(ClientId, CookieBasketId, BasketVersion);
        }

        [Fact]
        public async Task Checkout_WithEnforcementOn_ValidatesTheStoredCodeForThePrincipalsClient()
        {
            var fixture = new Fixture(Basket("SUMMER25", ClientId, BasketVersion));
            ValidatorAnswers(fixture, DiscountCodeValidation.Valid("SUMMER25"));

            var result = await fixture.CheckoutAsync();

            Assert.Equal((int)HttpStatusCode.Accepted, StatusOf(result));
            await fixture.Validator.Received(1).ValidateAsync(ClientId, "SUMMER25", Token, Arg.Any<CancellationToken>());
            await fixture.AssertCheckedOutAsync(ClientId, CookieBasketId, BasketVersion);
        }

        [Fact]
        public async Task Checkout_WithEnforcementOn_AndAStoredBasketWithoutAVersion_ReturnsConflict()
        {
            var fixture = new Fixture(Basket(version: null));

            var result = await fixture.CheckoutAsync();

            Assert.Equal((int)HttpStatusCode.Conflict, StatusOf(result));
            Assert.Equal("DiscountCodeBasketNeedsRefresh", MessageOf(result));
            await fixture.AssertNoCheckoutAsync();
        }

        [Theory]
        [InlineData(DiscountCodeValidationStatus.NotFound)]
        [InlineData(DiscountCodeValidationStatus.Disabled)]
        [InlineData(DiscountCodeValidationStatus.NotAssigned)]
        [InlineData(DiscountCodeValidationStatus.ClientUnknown)]
        [InlineData(DiscountCodeValidationStatus.NotApplicable)]
        public async Task Checkout_WithACodeThatIsNoLongerApplicable_ReturnsConflictAndDoesNotCheckOut(DiscountCodeValidationStatus status)
        {
            var fixture = new Fixture(Basket("SUMMER25", ClientId, BasketVersion));
            ValidatorAnswers(fixture, DiscountCodeValidation.Invalid(status));

            var result = await fixture.CheckoutAsync();

            Assert.Equal((int)HttpStatusCode.Conflict, StatusOf(result));
            Assert.Equal("DiscountCodeNoLongerValid", MessageOf(result));
            await fixture.AssertNoCheckoutAsync();
        }

        [Fact]
        public async Task Checkout_WhenVerificationIsUnavailable_Returns503AndDoesNotCheckOut()
        {
            var fixture = new Fixture(Basket("SUMMER25", ClientId, BasketVersion));
            ValidatorAnswers(fixture, DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Unavailable));

            var result = await fixture.CheckoutAsync();

            Assert.Equal((int)HttpStatusCode.ServiceUnavailable, StatusOf(result));
            Assert.Equal("DiscountCodeCouldNotBeVerified", MessageOf(result));
            await fixture.AssertNoCheckoutAsync();
        }

        [Fact]
        public async Task Checkout_WhenThePrincipalHasNoClient_CannotCheckOutWithACode()
        {
            var fixture = new Fixture(Basket("SUMMER25", ClientId, BasketVersion), withClient: false);
            ValidatorAnswers(fixture, DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.ClientUnknown));

            var result = await fixture.CheckoutAsync();

            Assert.Equal((int)HttpStatusCode.Conflict, StatusOf(result));
            await fixture.Validator.Received(1).ValidateAsync(null, "SUMMER25", Token, Arg.Any<CancellationToken>());
            await fixture.AssertNoCheckoutAsync();
        }

        [Fact]
        public async Task Checkout_WhenThePrincipalHasNoClient_KeepsTheRequestClientForAnOrderWithoutACode()
        {
            // No client could be resolved for the principal, by email or by organisation: the order is placed as before
            // enforcement. A client team member is not this case - they carry the client of their organisation.
            var fixture = new Fixture(Basket(version: BasketVersion), withClient: false);
            var bodyClientId = Guid.NewGuid();

            var result = await fixture.CheckoutAsync(bodyClientId);

            Assert.Equal((int)HttpStatusCode.Accepted, StatusOf(result));
            await fixture.Validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
            await fixture.AssertCheckedOutAsync(bodyClientId, CookieBasketId, BasketVersion);
        }

        [Fact]
        public async Task Checkout_WhenThePrincipalHasNoClient_NeverValidatesACodeForTheRequestClient()
        {
            var fixture = new Fixture(Basket("SUMMER25", ClientId, BasketVersion), withClient: false);
            ValidatorAnswers(fixture, DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.ClientUnknown));
            fixture.Validator.ValidateAsync(ClientId, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(DiscountCodeValidation.Valid("SUMMER25")));

            // Naming the client the code was saved for must not turn the request body into the validated identity.
            var result = await fixture.CheckoutAsync(bodyClientId: ClientId);

            Assert.Equal((int)HttpStatusCode.Conflict, StatusOf(result));
            await fixture.Validator.Received(1).ValidateAsync(null, "SUMMER25", Token, Arg.Any<CancellationToken>());
            await fixture.Validator.DidNotReceive().ValidateAsync(ClientId, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
            await fixture.AssertNoCheckoutAsync();
        }

        [Fact]
        public async Task Checkout_WhenTheBasketWasPricedForAnotherClient_ReturnsConflictEvenIfTheCodeIsAssignedToBoth()
        {
            var fixture = new Fixture(Basket("SUMMER25", Guid.NewGuid(), BasketVersion));
            ValidatorAnswers(fixture, DiscountCodeValidation.Valid("SUMMER25"));

            var result = await fixture.CheckoutAsync();

            Assert.Equal((int)HttpStatusCode.Conflict, StatusOf(result));
            Assert.Equal("DiscountCodeBasketNeedsRefresh", MessageOf(result));
            await fixture.AssertNoCheckoutAsync();
        }

        [Fact]
        public async Task Checkout_WhenTheStoredCodeIsNotTheCanonicalSpelling_ReturnsConflict()
        {
            var fixture = new Fixture(Basket("summer25", ClientId, BasketVersion));
            ValidatorAnswers(fixture, DiscountCodeValidation.Valid("SUMMER25"));

            var result = await fixture.CheckoutAsync();

            Assert.Equal((int)HttpStatusCode.Conflict, StatusOf(result));
            await fixture.AssertNoCheckoutAsync();
        }

        [Fact]
        public async Task Checkout_ForACustomOrderWithoutAStoredBasket_AssertsAbsenceWithTheEmptyVersion()
        {
            var fixture = new Fixture(storedBasket: null);

            var result = await fixture.CheckoutAsync(hasCustomOrder: true);

            Assert.Equal((int)HttpStatusCode.Accepted, StatusOf(result));
            await fixture.Validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
            await fixture.AssertCheckedOutAsync(ClientId, CookieBasketId, Guid.Empty);
        }

        private static DefaultHttpContext CreateHttpContext(Guid? clientId)
        {
            var claims = new List<Claim> { new(ClaimTypes.Email, "buyer@test.com") };

            if (clientId.HasValue)
            {
                claims.Add(new Claim(ClaimsEnrichmentConstants.ClientIdClaimType, clientId.Value.ToString()));
            }

            var context = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims, "test")) };
            context.Request.Headers.Cookie = $"{BasketConstants.BasketCookieName}={CookieBasketId}";

            var authentication = Substitute.For<IAuthenticationService>();
            var properties = new AuthenticationProperties();
            properties.StoreTokens(new[] { new AuthenticationToken { Name = "access_token", Value = Token } });
            authentication.AuthenticateAsync(Arg.Any<HttpContext>(), Arg.Any<string>())
                .Returns(Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(context.User, properties, "test"))));
            context.RequestServices = new ServiceCollection().AddSingleton(authentication).BuildServiceProvider();

            return context;
        }
    }
}
