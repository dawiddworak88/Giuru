using Buyer.Web.Areas.Orders.ApiControllers;
using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Orders.Repositories.UserApprovals;
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.Repositories.Identity;
using Buyer.Web.Shared.Services.Baskets;
using Foundation.Localization;
using Giuru.UnitTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BuyerBasket = Buyer.Web.Areas.Orders.DomainModels.Basket;

namespace Giuru.UnitTests.Orders.Baskets
{
    public class BuyerBasketCheckoutDiscountCodeTests
    {
        [Theory]
        [InlineData("current-client", HttpStatusCode.Accepted)]
        [InlineData("other-client", HttpStatusCode.BadRequest)]
        [InlineData("missing-proof", HttpStatusCode.BadRequest)]
        [InlineData("unavailable-basket", HttpStatusCode.BadRequest)]
        [InlineData("no-code", HttpStatusCode.Accepted)]
        public async Task Checkout_OnlyConsumesDiscountedPricesBoundToTheSignedInClient(string scenario, HttpStatusCode expectedStatus)
        {
            var basketId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var otherClientId = Guid.NewGuid();
            var context = BuyerFlowHelpers.CreateHttpContext(basketId, clientId);
            var code = scenario == "no-code" ? null : "STORED25";

            if (scenario == "current-client" || scenario == "other-client")
            {
                DiscountCodeBindingTestHelper.AddProof(context, basketId, scenario == "current-client" ? clientId : otherClientId, code);
            }

            var basketRepository = Substitute.For<IBasketRepository>();
            basketRepository.GetBasketById("token", Arg.Any<string>(), basketId)
                .Returns(Task.FromResult(scenario == "unavailable-basket" ? null : new BuyerBasket
                {
                    Id = basketId,
                    DiscountCode = code,
                    Items = new[] { new BasketItem { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 9m, Price = 9m } }
                }));
            var basketService = Substitute.For<IBasketService>();
            var controller = new BasketCheckoutApiController(
                basketRepository,
                basketService,
                Substitute.For<IClientAddressesRepository>(),
                TestLocalizer.Create<OrderResources>(),
                Substitute.For<IUserApprovalsRepository>(),
                Substitute.For<IIdentityRepository>(),
                DiscountCodeBindingTestHelper.Create(context))
            {
                ControllerContext = new ControllerContext { HttpContext = context }
            };

            // The body must not change the buyer whose binding is checked or the basket whose saved prices are consumed.
            var result = await controller.Checkout(new CheckoutBasketRequestModel { ClientId = otherClientId, BasketId = Guid.NewGuid() });

            Assert.Equal((int)expectedStatus, Assert.IsType<ObjectResult>(result).StatusCode);
            var checkouts = basketRepository.ReceivedCalls().Where(x => x.GetMethodInfo().Name == nameof(IBasketRepository.CheckoutBasketAsync)).ToList();

            if (expectedStatus == HttpStatusCode.Accepted)
            {
                var arguments = Assert.Single(checkouts).GetArguments();
                Assert.Equal(clientId, arguments[2]);
                Assert.Equal(basketId, arguments[5]);
                await basketService.Received(1).ValidateStockOutletQuantitiesAsync(basketId, "token", Arg.Any<string>());
            }
            else
            {
                Assert.Empty(checkouts);
                await basketService.DidNotReceiveWithAnyArgs().ValidateStockOutletQuantitiesAsync(default, default, default);
            }
        }
    }
}
