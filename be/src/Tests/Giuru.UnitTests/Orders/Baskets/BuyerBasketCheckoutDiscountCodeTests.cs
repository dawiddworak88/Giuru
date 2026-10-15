using Buyer.Web.Areas.Orders.ApiControllers;
using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Orders.Repositories.UserApprovals;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.Repositories.Identity;
using Buyer.Web.Shared.Services.Baskets;
using Foundation.Localization;
using Foundation.Pricing.DiscountCodes;
using Giuru.UnitTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BuyerBasket = Buyer.Web.Areas.Orders.DomainModels.Basket;

namespace Giuru.UnitTests.Orders.Baskets
{
    public class BuyerBasketCheckoutDiscountCodeTests
    {
        private static readonly Guid ClientId = Guid.NewGuid();

        private sealed record CheckoutFlow(Guid BasketId, DiscountFlowResult Flow, IBasketRepository BasketRepository, IBasketService BasketService);

        private static async Task<CheckoutFlow> CheckoutAsync(
            IDiscountCodeValidator validator,
            string storedDiscountCode,
            bool isBasketUnavailable = false,
            bool isGrulaConfigured = true)
        {
            var basketId = Guid.NewGuid();
            var context = BuyerFlowHelpers.CreateHttpContext(basketId, ClientId);
            var basketRepository = Substitute.For<IBasketRepository>();
            basketRepository.GetBasketById("token", Arg.Any<string>(), basketId)
                .Returns(Task.FromResult(isBasketUnavailable ? null : new BuyerBasket
                {
                    Id = basketId,
                    DiscountCode = storedDiscountCode,
                    Items = new[] { new BasketItem { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 9m, Price = 9m } }
                }));
            var basketService = Substitute.For<IBasketService>();
            var options = Options.Create(new AppSettings { GrulaAccessToken = isGrulaConfigured ? "test-token" : null, GrulaEnvironmentId = Guid.NewGuid().ToString() });
            var controller = new BasketCheckoutApiController(
                basketRepository,
                basketService,
                Substitute.For<IClientAddressesRepository>(),
                TestLocalizer.Create<OrderResources>(),
                Substitute.For<IUserApprovalsRepository>(),
                Substitute.For<IIdentityRepository>(),
                validator,
                options,
                Substitute.For<ILogger<BasketCheckoutApiController>>())
            {
                ControllerContext = new ControllerContext { HttpContext = context }
            };

            // The body must not change the buyer the code is validated for or the basket whose saved prices are consumed.
            var result = await controller.Checkout(new CheckoutBasketRequestModel { ClientId = Guid.NewGuid(), BasketId = Guid.NewGuid() });

            return new CheckoutFlow(basketId, new DiscountFlowResult { Result = result }, basketRepository, basketService);
        }

        private static async Task AssertOrderPlacedAsync(CheckoutFlow checkout)
        {
            Assert.Equal((int)HttpStatusCode.Accepted, checkout.Flow.StatusCode);

            var arguments = Assert.Single(CheckoutCalls(checkout)).GetArguments();
            Assert.Equal(ClientId, arguments[2]);
            Assert.Equal(checkout.BasketId, arguments[5]);
            await checkout.BasketService.Received(1).ValidateStockOutletQuantitiesAsync(checkout.BasketId, "token", Arg.Any<string>());
        }

        private static async Task AssertRefusedAsync(CheckoutFlow checkout, string expectedMessage)
        {
            Assert.Equal((int)HttpStatusCode.BadRequest, checkout.Flow.StatusCode);
            Assert.Equal(expectedMessage, checkout.Flow.Message);
            Assert.Empty(CheckoutCalls(checkout));
            await checkout.BasketService.DidNotReceiveWithAnyArgs().ValidateStockOutletQuantitiesAsync(default, default, default);
        }

        private static List<ICall> CheckoutCalls(CheckoutFlow checkout)
        {
            return checkout.BasketRepository.ReceivedCalls().Where(x => x.GetMethodInfo().Name == nameof(IBasketRepository.CheckoutBasketAsync)).ToList();
        }

        [Fact]
        public async Task Checkout_WhenTheStoredCodeIsStillApplicableForTheBuyersClient_PlacesTheOrder()
        {
            var validator = DiscountCodeValidators.AlwaysValid();

            var checkout = await CheckoutAsync(validator, "STORED25");

            await AssertOrderPlacedAsync(checkout);
            await validator.Received(1).ValidateAsync(ClientId, "STORED25", "token");
        }

        [Theory]
        [InlineData(DiscountCodeValidationStatus.Disabled)]
        [InlineData(DiscountCodeValidationStatus.NotFound)]
        [InlineData(DiscountCodeValidationStatus.NotAssigned)]
        [InlineData(DiscountCodeValidationStatus.ClientUnknown)]
        public async Task Checkout_WhenTheStoredCodeIsNoLongerApplicableForTheBuyersClient_IsRefused(DiscountCodeValidationStatus status)
        {
            // Revoked since the basket was last saved, or stored while the basket was another client's.
            var validator = DiscountCodeValidators.Returning(status);

            var checkout = await CheckoutAsync(validator, "STORED25");

            // The order would keep the discounted prices, so the buyer is told to remove the code, which reprices the basket.
            await AssertRefusedAsync(checkout, "DiscountCodeNoLongerValid");
            await validator.Received(1).ValidateAsync(ClientId, "STORED25", "token");
        }

        [Fact]
        public async Task Checkout_WhenTheStoredCodeCannotBeVerifiedBecauseTheLookupIsDown_PlacesTheOrder()
        {
            // The basket was priced with the code when it was applicable; an outage of the lookup does not block the order.
            var checkout = await CheckoutAsync(LookupDown.CreateValidator(), "STORED25");

            await AssertOrderPlacedAsync(checkout);
        }

        [Fact]
        public async Task Checkout_WhenTheBasketCannotBeRead_IsRefused()
        {
            var validator = DiscountCodeValidators.AlwaysValid();

            var checkout = await CheckoutAsync(validator, null, isBasketUnavailable: true);

            await AssertRefusedAsync(checkout, "BasketPricesCouldNotBeVerified");
        }

        [Fact]
        public async Task Checkout_OfABasketWithoutACode_DoesNotValidateAnything()
        {
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.Unavailable);

            var checkout = await CheckoutAsync(validator, null);

            await AssertOrderPlacedAsync(checkout);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }

        [Fact]
        public async Task Checkout_WhenGrulaIsNotConfigured_DoesNotValidateTheUnpricedCode()
        {
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.Disabled);

            var checkout = await CheckoutAsync(validator, "STORED25", isGrulaConfigured: false);

            await AssertOrderPlacedAsync(checkout);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }
    }
}
