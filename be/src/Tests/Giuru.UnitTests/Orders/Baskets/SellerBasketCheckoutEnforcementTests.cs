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
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Seller.Web.Areas.Inventory.Repositories;
using Seller.Web.Areas.Inventory.Repositories.Inventories;
using Seller.Web.Areas.Orders.ApiControllers;
using Seller.Web.Areas.Orders.ApiRequestModels;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.Repositories.Baskets;
using Seller.Web.Areas.Orders.Services.Basket;
using Seller.Web.Areas.Shared.Repositories.UserApprovals;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.Repositories.Clients;
using Seller.Web.Shared.Repositories.Identity;
using SellerClient = Seller.Web.Areas.Clients.DomainModels.Client;
using SellerBasket = Seller.Web.Areas.Orders.DomainModels.Basket;

namespace Giuru.UnitTests.Orders.Baskets
{
    // Seller checkout: the order is placed for model.ClientId, so that is the client the stored code has to be applicable for,
    // and the stored basket has to be the snapshot that was priced for that same client.
    public class SellerBasketCheckoutEnforcementTests
    {
        private const string Token = "token";
        private static readonly Guid BasketId = Guid.NewGuid();
        private static readonly Guid ClientId = Guid.NewGuid();
        private static readonly Guid BasketVersion = Guid.NewGuid();

        private sealed class Fixture
        {
            public IBasketRepository BasketRepository { get; } = Substitute.For<IBasketRepository>();
            public IDiscountCodeValidator Validator { get; } = Substitute.For<IDiscountCodeValidator>();
            public IClientsRepository ClientsRepository { get; } = Substitute.For<IClientsRepository>();
            public BasketCheckoutApiController Controller { get; }

            public Fixture(SellerBasket basket, bool grulaConfigured = true, bool enforcementEnabled = true)
            {
                BasketRepository.GetBasketByIdAsync(Arg.Any<string>(), Arg.Any<string>(), BasketId).Returns(Task.FromResult(basket));
                ClientsRepository.GetClientAsync(Arg.Any<string>(), Arg.Any<string>(), ClientId)
                    .Returns(Task.FromResult(new SellerClient { Email = "client@test.com", OrganisationId = Guid.NewGuid() }));

                var options = Options.Create(new AppSettings
                {
                    GrulaAccessToken = grulaConfigured ? "grula-token" : null,
                    GrulaEnvironmentId = Guid.NewGuid().ToString(),
                    DiscountCodeEnforcementEnabled = enforcementEnabled
                });

                Controller = new BasketCheckoutApiController(
                    BasketRepository,
                    TestLocalizer.Create<OrderResources>(),
                    TestLocalizer.Create<ClientResources>(),
                    Substitute.For<IUserApprovalsRepository>(),
                    ClientsRepository,
                    Substitute.For<IIdentityRepository>(),
                    Substitute.For<IBasketService>(),
                    Substitute.For<IInventoryRepository>(),
                    Substitute.For<IOutletRepository>(),
                    options,
                    Validator)
                {
                    ControllerContext = new ControllerContext { HttpContext = CreateHttpContext() }
                };
            }

            public Task<IActionResult> CheckoutAsync(Guid? clientId = null)
            {
                return Controller.Checkout(new CheckoutBasketRequestModel { BasketId = BasketId, ClientId = clientId ?? ClientId });
            }

            public Task AssertNoCheckoutAsync()
            {
                return BasketRepository.DidNotReceiveWithAnyArgs().CheckoutBasketAsync(
                    default, default, default, default, default, default, default, default, default, default, default, default, default,
                    default, default, default, default, default, default, default, default, default, default, default, default, default,
                    default, default, default, default);
            }

            public Task AssertCheckedOutWithVersionAsync(Guid? expectedVersion)
            {
                return BasketRepository.Received(1).CheckoutBasketAsync(
                    Token, Arg.Any<string>(), ClientId, Arg.Any<string>(), Arg.Any<string>(), BasketId,
                    Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                    Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<Guid?>(), Arg.Any<string>(),
                    Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                    Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<Guid?>(), expectedVersion);
            }
        }

        private static SellerBasket Basket(string discountCode = null, Guid? discountCodeClientId = null, Guid? version = null)
        {
            return new SellerBasket
            {
                Id = BasketId,
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
            return (string)((ObjectResult)result).Value.GetType().GetProperty("Message").GetValue(((ObjectResult)result).Value);
        }

        [Fact]
        public async Task Checkout_WithEnforcementOff_ValidatesNothingAndSendsNoVersion()
        {
            var fixture = new Fixture(Basket("SUMMER25"), enforcementEnabled: false);

            var result = await fixture.CheckoutAsync();

            Assert.Equal((int)HttpStatusCode.Accepted, StatusOf(result));
            await fixture.Validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
            await fixture.AssertCheckedOutWithVersionAsync(null);
        }

        [Fact]
        public async Task Checkout_WhenGrulaIsNotConfigured_ValidatesNothingAndSendsNoVersion()
        {
            var fixture = new Fixture(Basket("SUMMER25"), grulaConfigured: false);

            var result = await fixture.CheckoutAsync();

            Assert.Equal((int)HttpStatusCode.Accepted, StatusOf(result));
            await fixture.Validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
            await fixture.AssertCheckedOutWithVersionAsync(null);
        }

        [Fact]
        public async Task Checkout_WithEnforcementOn_AndABasketWithoutAVersion_ReturnsConflictAndDoesNotCheckOut()
        {
            var fixture = new Fixture(Basket(version: null));

            var result = await fixture.CheckoutAsync();

            Assert.Equal((int)HttpStatusCode.Conflict, StatusOf(result));
            Assert.Equal("DiscountCodeBasketNeedsRefresh", MessageOf(result));
            await fixture.AssertNoCheckoutAsync();
        }

        [Fact]
        public async Task Checkout_WithEnforcementOn_AndNoCode_SendsTheVersionWithoutValidating()
        {
            // A concurrent save could add a code between this read and the order, so even a code-free basket is guarded.
            var fixture = new Fixture(Basket(version: BasketVersion));

            var result = await fixture.CheckoutAsync();

            Assert.Equal((int)HttpStatusCode.Accepted, StatusOf(result));
            await fixture.Validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
            await fixture.AssertCheckedOutWithVersionAsync(BasketVersion);
        }

        [Fact]
        public async Task Checkout_WithAValidCodeForTheSameClient_ProceedsAndValidatesForTheCheckoutClient()
        {
            var fixture = new Fixture(Basket("SUMMER25", ClientId, BasketVersion));
            ValidatorAnswers(fixture, DiscountCodeValidation.Valid("SUMMER25"));

            var result = await fixture.CheckoutAsync();

            Assert.Equal((int)HttpStatusCode.Accepted, StatusOf(result));
            await fixture.Validator.Received(1).ValidateAsync(ClientId, "SUMMER25", Token, Arg.Any<CancellationToken>());
            await fixture.AssertCheckedOutWithVersionAsync(BasketVersion);
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
        public async Task Checkout_WhenTheBasketWasPricedForAnotherClient_ReturnsConflictEvenIfBothClientsAreAssignedTheCode()
        {
            var otherClientId = Guid.NewGuid();
            var fixture = new Fixture(Basket("SUMMER25", otherClientId, BasketVersion));
            ValidatorAnswers(fixture, DiscountCodeValidation.Valid("SUMMER25"));

            var result = await fixture.CheckoutAsync();

            Assert.Equal((int)HttpStatusCode.Conflict, StatusOf(result));
            Assert.Equal("DiscountCodeBasketNeedsRefresh", MessageOf(result));
            await fixture.AssertNoCheckoutAsync();
        }

        [Fact]
        public async Task Checkout_WhenTheBasketHasNoStoredClient_ReturnsConflict()
        {
            // A discounted basket saved before the client metadata existed must be saved once for the checkout client.
            var fixture = new Fixture(Basket("SUMMER25", discountCodeClientId: null, version: BasketVersion));
            ValidatorAnswers(fixture, DiscountCodeValidation.Valid("SUMMER25"));

            var result = await fixture.CheckoutAsync();

            Assert.Equal((int)HttpStatusCode.Conflict, StatusOf(result));
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

        private static DefaultHttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) };
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
