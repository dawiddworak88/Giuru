using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using Buyer.Web.Areas.Orders.DomainModels;
using Catalog.Api.v1.Products.RequestModels;
using Foundation.ApiExtensions.Models.Response;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Paginations;
using Giuru.IntegrationTests.Definitions;
using Giuru.IntegrationTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
    [Collection(nameof(ApiCollection))]
    public class CommonTests
    {
        private readonly ApiFixture _apiFixture;

        public CommonTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        [Fact]
        public async Task CreateProducts_AddProductsToInventory_CheckoutOrder_Returns_Orders()
        {
            var onStockProduct = await TestsHelper.CreateProductAndAddToStockAsync(_apiFixture, new ProductRequestModel
            {
                Sku = Products.Anton.Sku,
                Name = Products.Anton.Name,
                CategoryId = Products.Anton.CategoryId,
                IsPublished = Products.Anton.IsPublished,
                Ean = Products.Anton.Ean,
            },
            ApiEndpoints.InventoriesApiEndpoint);

            var onOutletProduct = await TestsHelper.CreateProductAndAddToStockAsync(_apiFixture, new ProductRequestModel
            {
                Sku = Products.Aga.Sku,
                Name = Products.Aga.Name,
                CategoryId = Products.Aga.CategoryId,
                IsPublished = Products.Aga.IsPublished,
                Ean = Products.Aga.Ean,
            },
            ApiEndpoints.OutletsApiEndpoint);

            var basket = await _apiFixture.BuyerWebClient.PostAsync<SaveBasketRequestModel, BasketResponseModel>(ApiEndpoints.BasketApiEndpoint, new SaveBasketRequestModel
            {
                Items = new List<BasketItemRequestModel>()
                {
                    new BasketItemRequestModel
                    {
                        ProductId = onStockProduct,
                        Sku = Products.Anton.Sku,
                        Name = Products.Anton.Name,
                        StockQuantity = Products.Quantities.AvailableQuantity
                    },
                    new BasketItemRequestModel
                    {
                        ProductId = onOutletProduct,
                        Sku = Products.Aga.Sku,
                        Name = Products.Aga.Name,
                        OutletQuantity = Products.Quantities.AvailableQuantity
                    }
                }
            });

            Assert.NotNull(basket);
            Assert.NotEqual(Guid.Empty, basket.Id);
            Assert.NotNull(basket.Items);
            Assert.NotNull(basket.Items.FirstOrDefault(x => x.ProductId == onStockProduct));
            Assert.NotNull(basket.Items.FirstOrDefault(x => x.ProductId == onOutletProduct));

            await _apiFixture.BuyerWebClient.PostAsync<CheckoutBasketRequestModel, BaseResponseModel>(ApiEndpoints.OrderCheckoutApiEndpoint, new CheckoutBasketRequestModel
            {
                BasketId = basket.Id,
                ClientId = Clients.Id,
                ClientName = Clients.Name
            });

            var getResults = await TestsHelper.GetDataAsync(
                () => _apiFixture.BuyerWebClient.GetAsync<PagedResults<IEnumerable<Order>>>($"{ApiEndpoints.GetOrdersApiEndpoint}?pageIndex={Constants.DefaultPageIndex}&itemsPerPage={Constants.DefaultItemsPerPage}"),
                x => x.OrderItems.Any(y => y.ProductId == onStockProduct || y.ProductId == onOutletProduct));

            var order = getResults.Data.FirstOrDefault(x => x.OrderItems.Any(x => x.ProductId == onStockProduct || x.ProductId == onOutletProduct));

            Assert.NotNull(order);
            Assert.NotEmpty(order.OrderItems);

            var stockOrderItem = order.OrderItems.FirstOrDefault(x => x.ProductId == onStockProduct);

            Assert.NotNull(stockOrderItem);
            Assert.Equal(Products.Quantities.AvailableQuantity, stockOrderItem.StockQuantity);

            var outletOrderItem = order.OrderItems.FirstOrDefault(x => x.ProductId == onOutletProduct);

            Assert.NotNull(outletOrderItem);
            Assert.Equal(Products.Quantities.AvailableQuantity, outletOrderItem.OutletQuantity);
        }
    }
}
