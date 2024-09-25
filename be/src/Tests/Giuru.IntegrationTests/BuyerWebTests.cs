using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using Buyer.Web.Areas.Orders.DomainModels;
using Foundation.ApiExtensions.Models.Response;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Paginations;
using Giuru.IntegrationTests.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
    [Collection(nameof(ApiCollection))]
    public class BuyerWebTests
    {
        private readonly ApiFixture _apiFixture;

        public BuyerWebTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        [Fact]
        public async Task AddToBasketAndCheckoutOrder_Returns_Orders()
        {
            var basket = await _apiFixture.BuyerWebClient.PostAsync<SaveBasketRequestModel, BasketResponseModel>(ApiEndpoints.BasketApiEndpoint, new SaveBasketRequestModel
            {
                Items = new List<BasketItemRequestModel>()
                {
                    new BasketItemRequestModel
                    {
                        ProductId = Products.Lamica.Id,
                        Sku = Products.Lamica.Sku,
                        Name = Products.Lamica.Name,
                        Quantity = 2,
                        StockQuantity = 1
                    }
                }
            });

            Assert.NotNull(basket);
            Assert.NotEqual(Guid.Empty, basket.Id);
            Assert.NotNull(basket.Items);
            Assert.Equal(Products.Lamica.Id, basket.Items.FirstOrDefault().ProductId);

            await _apiFixture.BuyerWebClient.PostAsync<CheckoutBasketRequestModel, BaseResponseModel>(ApiEndpoints.OrderCheckoutApiEndpoint, new CheckoutBasketRequestModel
            {
                BasketId = basket.Id,
                ClientId = Clients.Id,
                ClientName = Clients.Name
            });

            var orders = await _apiFixture.BuyerWebClient.GetAsync<PagedResults<IEnumerable<Order>>>($"{ApiEndpoints.GetOrdersApiEndpoint}?pageIndex={Constants.DefaultPageIndex}&itemsPerPage={Constants.DefaultItemsPerPage}");

            Assert.NotNull(orders.Data);
            Assert.Equal(1, orders.Total);
            Assert.Equal(Products.Lamica.Id, orders.Data.FirstOrDefault().OrderItems.FirstOrDefault().ProductId);
        }
    }
}
