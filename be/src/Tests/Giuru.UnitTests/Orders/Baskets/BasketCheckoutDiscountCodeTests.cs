using Buyer.Web.Areas.Orders.ApiControllers;
using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.Repositories.UserApprovals;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Basket;
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.Repositories.Identity;
using Buyer.Web.Shared.Services.Baskets;
using Buyer.Web.Shared.Services.Prices;
using Foundation.Localization;
using Foundation.Pricing.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BuyerBasket = Buyer.Web.Areas.Orders.DomainModels.Basket;
using BuyerClaimsEnrichmentConstants = Buyer.Web.Shared.Definitions.Middlewares.ClaimsEnrichmentConstants;
using BuyerIBasketRepository = Buyer.Web.Areas.Orders.Repositories.Baskets.IBasketRepository;
using SellerAppSettings = Seller.Web.Shared.Configurations.AppSettings;
using SellerBasket = Seller.Web.Areas.Orders.DomainModels.Basket;
using SellerBasketCheckoutApiController = Seller.Web.Areas.Orders.ApiControllers.BasketCheckoutApiController;
using SellerBasketItem = Seller.Web.Areas.Orders.DomainModels.BasketItem;
using SellerCheckoutBasketRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.CheckoutBasketRequestModel;
using SellerClient = Seller.Web.Areas.Clients.DomainModels.Client;
using SellerClientResources = Foundation.Localization.ClientResources;
using SellerIBasketRepository = Seller.Web.Areas.Orders.Repositories.Baskets.IBasketRepository;
using SellerIClientsRepository = Seller.Web.Shared.Repositories.Clients.IClientsRepository;
using SellerIIdentityRepository = Seller.Web.Shared.Repositories.Identity.IIdentityRepository;
using SellerIUserApprovalsRepository = Seller.Web.Areas.Shared.Repositories.UserApprovals.IUserApprovalsRepository;

namespace Giuru.UnitTests.Orders.Baskets
{
    // Checkout re-validates the Applied Discount Code against the Client being checked out, so a
    // withdrawn code can never be ordered at a discounted price. Driven through the controller
    // action; the only observable outcomes are the response and whether the Basket was checked out.
    public class BuyerBasketCheckoutDiscountCodeTests
    {
        private static readonly Guid ClientId = Guid.NewGuid();
        private static readonly Guid BasketId = Guid.NewGuid();

        [Fact]
        public async Task Checkout_WhenTheRequestClientIsNotTheSignedInClient_IsRefusedAndNothingIsCheckedOut()
        {
            var basketRepository = CreateBasketRepository("SUMMER25");
            var controller = CreateController(basketRepository, Substitute.For<IApplicableDiscountCodeService>());

            var result = await controller.Checkout(new CheckoutBasketRequestModel { ClientId = Guid.NewGuid() });

            Assert.Equal(StatusCodes.Status400BadRequest, Assert.IsType<ObjectResult>(result).StatusCode);
            await AssertNotCheckedOut(basketRepository);
        }

        [Fact]
        public async Task Checkout_WhenTheAppliedDiscountCodeIsNoLongerValid_IsRefusedAndNothingIsCheckedOut()
        {
            var basketRepository = CreateBasketRepository("SUMMER25");
            var discountCodes = Substitute.For<IApplicableDiscountCodeService>();
            discountCodes.GetValidAsync(Arg.Any<string>(), "SUMMER25").Returns(Task.FromResult<string>(null));
            var controller = CreateController(basketRepository, discountCodes);

            var result = await controller.Checkout(new CheckoutBasketRequestModel { ClientId = ClientId });

            Assert.Equal(StatusCodes.Status400BadRequest, Assert.IsType<ObjectResult>(result).StatusCode);
            await AssertNotCheckedOut(basketRepository);
        }

        [Fact]
        public async Task Checkout_WhenTheAppliedDiscountCodeIsStillValid_PlacesTheOrder()
        {
            var basketRepository = CreateBasketRepository("SUMMER25");
            var controller = CreateController(basketRepository, DiscountCodeEligibilityStubs.AcceptingForBuyer());

            var result = await controller.Checkout(new CheckoutBasketRequestModel { ClientId = ClientId });

            Assert.Equal(StatusCodes.Status202Accepted, Assert.IsType<ObjectResult>(result).StatusCode);
            await AssertCheckedOut(basketRepository);
        }

        [Fact]
        public async Task Checkout_WhenTheBasketHasNoDiscountCode_PlacesTheOrderWithoutCheckingAnyCode()
        {
            var basketRepository = CreateBasketRepository(null);
            var discountCodes = Substitute.For<IApplicableDiscountCodeService>();
            var controller = CreateController(basketRepository, discountCodes);

            var result = await controller.Checkout(new CheckoutBasketRequestModel { ClientId = ClientId });

            Assert.Equal(StatusCodes.Status202Accepted, Assert.IsType<ObjectResult>(result).StatusCode);
            await discountCodes.DidNotReceive().GetValidAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        private static BuyerIBasketRepository CreateBasketRepository(string discountCode)
        {
            var basketRepository = Substitute.For<BuyerIBasketRepository>();

            basketRepository.GetBasketById(Arg.Any<string>(), Arg.Any<string>(), BasketId)
                .Returns(Task.FromResult(new BuyerBasket { Id = BasketId, DiscountCode = discountCode }));

            return basketRepository;
        }

        private static BasketCheckoutApiController CreateController(BuyerIBasketRepository basketRepository, IApplicableDiscountCodeService discountCodes)
        {
            var localizer = Substitute.For<IStringLocalizer<OrderResources>>();
            localizer[Arg.Any<string>()].Returns(call => new LocalizedString(call.ArgAt<string>(0), call.ArgAt<string>(0)));
            var options = Options.Create(new AppSettings
            {
                GrulaAccessToken = "test-token",
                GrulaEnvironmentId = Guid.NewGuid().ToString()
            });

            var context = new DefaultHttpContext();
            context.Request.Headers.Cookie = $"{BasketConstants.BasketCookieName}={BasketId}";
            context.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(BuyerClaimsEnrichmentConstants.ClientIdClaimType, ClientId.ToString())
            }, "test"));
            var authentication = Substitute.For<IAuthenticationService>();
            var properties = new AuthenticationProperties();
            properties.StoreTokens(new[] { new AuthenticationToken { Name = "access_token", Value = "token" } });
            authentication.AuthenticateAsync(Arg.Any<HttpContext>(), Arg.Any<string>())
                .Returns(Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(context.User, properties, "test"))));
            context.RequestServices = new ServiceCollection().AddSingleton(authentication).BuildServiceProvider();

            return new BasketCheckoutApiController(
                basketRepository,
                Substitute.For<IBasketService>(),
                Substitute.For<IClientAddressesRepository>(),
                localizer,
                Substitute.For<IUserApprovalsRepository>(),
                Substitute.For<IIdentityRepository>(),
                discountCodes,
                options)
            {
                ControllerContext = new ControllerContext { HttpContext = context }
            };
        }

        private static async Task AssertCheckedOut(BuyerIBasketRepository basketRepository)
        {
            Assert.Single(basketRepository.ReceivedCalls(), x => x.GetMethodInfo().Name == nameof(BuyerIBasketRepository.CheckoutBasketAsync));
            await Task.CompletedTask;
        }

        private static async Task AssertNotCheckedOut(BuyerIBasketRepository basketRepository)
        {
            Assert.DoesNotContain(basketRepository.ReceivedCalls(), x => x.GetMethodInfo().Name == nameof(BuyerIBasketRepository.CheckoutBasketAsync));
            await Task.CompletedTask;
        }
    }

    public class SellerBasketCheckoutDiscountCodeTests
    {
        private static readonly Guid ClientId = Guid.NewGuid();
        private static readonly Guid BasketId = Guid.NewGuid();

        [Fact]
        public async Task Checkout_WhenTheDiscountCodeIsNotValidForTheCheckoutClient_IsRefusedAndNothingIsCheckedOut()
        {
            var basketRepository = CreateBasketRepository("SUMMER25");
            // The code belongs to another Client, so a Basket priced for that Client cannot be ordered here.
            var eligibility = DiscountCodeEligibilityStubs.KnowingFor(Guid.NewGuid(), "SUMMER25");
            var controller = CreateController(basketRepository, eligibility);

            var result = await controller.Checkout(new SellerCheckoutBasketRequestModel { BasketId = BasketId, ClientId = ClientId });

            Assert.Equal(StatusCodes.Status400BadRequest, Assert.IsType<ObjectResult>(result).StatusCode);
            Assert.DoesNotContain(basketRepository.ReceivedCalls(), x => x.GetMethodInfo().Name == nameof(SellerIBasketRepository.CheckoutBasketAsync));
        }

        [Fact]
        public async Task Checkout_WhenTheDiscountCodeIsValidForTheCheckoutClient_PlacesTheOrder()
        {
            var basketRepository = CreateBasketRepository("SUMMER25");
            var eligibility = DiscountCodeEligibilityStubs.KnowingFor(ClientId, "SUMMER25");
            var controller = CreateController(basketRepository, eligibility);

            var result = await controller.Checkout(new SellerCheckoutBasketRequestModel { BasketId = BasketId, ClientId = ClientId });

            Assert.Equal(StatusCodes.Status202Accepted, Assert.IsType<ObjectResult>(result).StatusCode);
            Assert.Single(basketRepository.ReceivedCalls(), x => x.GetMethodInfo().Name == nameof(SellerIBasketRepository.CheckoutBasketAsync));
        }

        [Fact]
        public async Task Checkout_WhenTheBasketHasNoDiscountCode_PlacesTheOrderWithoutCheckingAnyCode()
        {
            var basketRepository = CreateBasketRepository(null);
            var eligibility = Substitute.For<IDiscountCodeEligibilityRepository>();
            var controller = CreateController(basketRepository, eligibility);

            var result = await controller.Checkout(new SellerCheckoutBasketRequestModel { BasketId = BasketId, ClientId = ClientId });

            Assert.Equal(StatusCodes.Status202Accepted, Assert.IsType<ObjectResult>(result).StatusCode);
            await eligibility.DidNotReceive().GetValidDiscountCodeAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<string>());
        }

        private static SellerIBasketRepository CreateBasketRepository(string discountCode)
        {
            var basketRepository = Substitute.For<SellerIBasketRepository>();

            basketRepository.GetBasketByIdAsync(Arg.Any<string>(), Arg.Any<string>(), BasketId)
                .Returns(Task.FromResult(new SellerBasket
                {
                    Id = BasketId,
                    DiscountCode = discountCode,
                    Items = new[] { new SellerBasketItem { ProductId = Guid.NewGuid(), Quantity = 1 } }
                }));

            return basketRepository;
        }

        private static SellerBasketCheckoutApiController CreateController(SellerIBasketRepository basketRepository, IDiscountCodeEligibilityRepository eligibility)
        {
            var orderLocalizer = Substitute.For<IStringLocalizer<OrderResources>>();
            orderLocalizer[Arg.Any<string>()].Returns(call => new LocalizedString(call.ArgAt<string>(0), call.ArgAt<string>(0)));
            var clientsRepository = Substitute.For<SellerIClientsRepository>();
            clientsRepository.GetClientAsync(Arg.Any<string>(), Arg.Any<string>(), ClientId)
                .Returns(Task.FromResult(new SellerClient { Id = ClientId, Email = "client@example.com", OrganisationId = Guid.NewGuid() }));
            var options = Options.Create(new SellerAppSettings
            {
                GrulaAccessToken = "test-token",
                GrulaEnvironmentId = Guid.NewGuid().ToString()
            });

            var context = new DefaultHttpContext();
            var authentication = Substitute.For<IAuthenticationService>();
            var properties = new AuthenticationProperties();
            properties.StoreTokens(new[] { new AuthenticationToken { Name = "access_token", Value = "token" } });
            authentication.AuthenticateAsync(Arg.Any<HttpContext>(), Arg.Any<string>())
                .Returns(Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(), properties, "test"))));
            context.RequestServices = new ServiceCollection().AddSingleton(authentication).BuildServiceProvider();

            return new SellerBasketCheckoutApiController(
                basketRepository,
                orderLocalizer,
                Substitute.For<IStringLocalizer<SellerClientResources>>(),
                Substitute.For<SellerIUserApprovalsRepository>(),
                clientsRepository,
                Substitute.For<SellerIIdentityRepository>(),
                Substitute.For<Seller.Web.Areas.Orders.Services.Basket.IBasketService>(),
                Substitute.For<Seller.Web.Areas.Inventory.Repositories.Inventories.IInventoryRepository>(),
                Substitute.For<Seller.Web.Areas.Inventory.Repositories.IOutletRepository>(),
                eligibility,
                options)
            {
                ControllerContext = new ControllerContext { HttpContext = context }
            };
        }
    }

    // The buyer-side check: the Client is always the one in the signed-in user's claims.
    public class ApplicableDiscountCodeServiceTests
    {
        private static readonly Guid ClientId = Guid.NewGuid();

        [Fact]
        public async Task GetValidAsync_WhenTheCodeIsValidForTheSignedInClient_ReturnsItsStoredSpelling()
        {
            var eligibility = Substitute.For<IDiscountCodeEligibilityRepository>();
            eligibility.GetValidDiscountCodeAsync("token", Arg.Any<string>(), ClientId, "summer25").Returns(Task.FromResult("SUMMER25"));
            var service = CreateService(eligibility, true, ClientId);

            Assert.Equal("SUMMER25", await service.GetValidAsync("token", "summer25"));
        }

        [Fact]
        public async Task GetValidAsync_WhenGrulaIsNotConfigured_ReturnsNullWithoutCallingClientApi()
        {
            var eligibility = Substitute.For<IDiscountCodeEligibilityRepository>();
            var service = CreateService(eligibility, false, ClientId);

            Assert.Null(await service.GetValidAsync("token", "SUMMER25"));
            await eligibility.DidNotReceive().GetValidDiscountCodeAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<string>());
        }

        [Fact]
        public async Task GetValidAsync_WhenNoClientIsSignedIn_ReturnsNullWithoutCallingClientApi()
        {
            var eligibility = Substitute.For<IDiscountCodeEligibilityRepository>();
            var service = CreateService(eligibility, true, null);

            Assert.Null(await service.GetValidAsync("token", "SUMMER25"));
            await eligibility.DidNotReceive().GetValidDiscountCodeAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<string>());
        }

        [Fact]
        public async Task GetValidAsync_WhenThereIsNoCode_ReturnsNullWithoutCallingClientApi()
        {
            var eligibility = Substitute.For<IDiscountCodeEligibilityRepository>();
            var service = CreateService(eligibility, true, ClientId);

            Assert.Null(await service.GetValidAsync("token", "  "));
            await eligibility.DidNotReceive().GetValidDiscountCodeAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<string>());
        }

        private static ApplicableDiscountCodeService CreateService(IDiscountCodeEligibilityRepository eligibility, bool grulaConfigured, Guid? clientId)
        {
            var claims = clientId.HasValue
                ? new[] { new Claim(BuyerClaimsEnrichmentConstants.ClientIdClaimType, clientId.Value.ToString()) }
                : Array.Empty<Claim>();
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims, "test")) });
            var options = Options.Create(new AppSettings
            {
                GrulaAccessToken = grulaConfigured ? "test-token" : null,
                GrulaEnvironmentId = grulaConfigured ? Guid.NewGuid().ToString() : null
            });

            return new ApplicableDiscountCodeService(httpContextAccessor, eligibility, options);
        }
    }
}
