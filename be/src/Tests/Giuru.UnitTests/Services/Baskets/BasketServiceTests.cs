using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Products.Repositories;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Shared.Services.Baskets;
using Buyer.Web.Shared.Services.Prices;
using Foundation.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Services.Baskets
{
    public class BasketServiceTests
    {
        [Fact]
        public async Task GetBasketAsync_WhenTheCurrentClientCannotSeePrices_MasksPricesWithoutMutatingThePersistedBasket()
        {
            var basketId = Guid.NewGuid();
            var persistedItem = new BasketItem
            {
                ProductId = Guid.NewGuid(), ProductSku = "SKU", ProductName = "Product", Quantity = 2,
                UnitPrice = 10m, Price = 20m, Currency = "EUR"
            };
            var basketRepository = Substitute.For<IBasketRepository>();
            var priceService = Substitute.For<IPriceService>();
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            var context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            };

            basketRepository.GetBasketById("token", "en", basketId)
                .Returns(Task.FromResult(new Basket { Id = basketId, Items = new[] { persistedItem } }));
            priceService.CanSeePrices(Arg.Any<Guid?>()).Returns(false);
            httpContextAccessor.HttpContext.Returns(context);

            var service = new BasketService(
                Substitute.For<LinkGenerator>(),
                basketRepository,
                Substitute.For<IInventoryRepository>(),
                Substitute.For<IOutletRepository>(),
                Substitute.For<IStringLocalizer<OrderResources>>(),
                Substitute.For<IStringLocalizer<GlobalResources>>(),
                priceService,
                httpContextAccessor);

            var basket = await service.GetBasketAsync(basketId, "token", "en");

            var returnedItem = Assert.Single(basket.Items);
            Assert.Null(returnedItem.UnitPrice);
            Assert.Null(returnedItem.Price);
            Assert.Null(returnedItem.Currency);
            Assert.Equal(10m, persistedItem.UnitPrice);
            Assert.Equal(20m, persistedItem.Price);
            Assert.Equal("EUR", persistedItem.Currency);
        }
    }
}