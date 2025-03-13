using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using Buyer.Web.Areas.Orders.DomainModels;
using Catalog.Api.v1.Products.RequestModels;
using Foundation.ApiExtensions.Models.Response;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Paginations;
using Giuru.IntegrationTests.Definitions;
using Inventory.Api.v1.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            var onStockProduct = await _apiFixture.SellerWebClient.PostAsync<ProductRequestModel, BaseResponseModel>(ApiEndpoints.ProductsApiEndpoint, new ProductRequestModel
            {
                Sku = Products.Anton.Sku,
                Name = Products.Anton.Name,
                CategoryId = Products.Anton.CategoryId,
                IsPublished = Products.Anton.IsPublished,
                Ean = Products.Anton.Ean,
            });

            Assert.NotNull(onStockProduct);
            Assert.NotEqual(Guid.Empty, onStockProduct.Id);

            var addStockProduct = await _apiFixture.SellerWebClient.PostAsync<InventoryRequestModel, BaseResponseModel>(ApiEndpoints.InventoriesApiEndpoint, new InventoryRequestModel
            {
                ProductId = onStockProduct.Id,
                WarehouseId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                AvailableQuantity = Products.Anton.AvailableQuantity,
                Quantity = Products.Anton.Quantity,
            });

            Assert.NotNull(addStockProduct);
            Assert.NotEqual(Guid.Empty, addStockProduct.Id);

            var onOutletProduct = await _apiFixture.SellerWebClient.PostAsync<ProductRequestModel, BaseResponseModel>(ApiEndpoints.ProductsApiEndpoint, new ProductRequestModel
            {
                Name = Products.Aga.Name,
                Sku = Products.Aga.Sku,
                CategoryId = Products.Aga.CategoryId,
                IsPublished = Products.Aga.IsPublished,
                Ean = Products.Aga.Ean,
            });

            Assert.NotNull(onOutletProduct);
            Assert.NotEqual(Guid.Empty, onOutletProduct.Id);

            var addOutletProduct = await _apiFixture.SellerWebClient.PostAsync<OutletRequestModel, BaseResponseModel>(ApiEndpoints.OutletsApiEndpoint, new OutletRequestModel
            {
                ProductId = onOutletProduct.Id,
                WarehouseId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                AvailableQuantity = Products.Aga.AvailableQuantity,
                Quantity = Products.Aga.Quantity,
            });

            Assert.NotNull(addOutletProduct);
            Assert.NotEqual(Guid.Empty, addOutletProduct.Id);

            var basket = await _apiFixture.BuyerWebClient.PostAsync<SaveBasketRequestModel, BasketResponseModel>(ApiEndpoints.BasketApiEndpoint, new SaveBasketRequestModel
            {
                Items = new List<BasketItemRequestModel>()
                {
                    new BasketItemRequestModel
                    {
                        ProductId = onStockProduct.Id,
                        Sku = Products.Anton.Sku,
                        Name = Products.Anton.Name,
                        StockQuantity = Products.Anton.AvailableQuantity
                    },
                    new BasketItemRequestModel
                    {
                        ProductId = onOutletProduct.Id,
                        Sku = Products.Aga.Sku,
                        Name = Products.Aga.Name,
                        OutletQuantity = Products.Aga.AvailableQuantity
                    }
                }
            });

            Assert.NotNull(basket);
            Assert.NotEqual(Guid.Empty, basket.Id);
            Assert.NotNull(basket.Items);
            Assert.NotNull(basket.Items.FirstOrDefault(x => x.ProductId == onStockProduct.Id));
            Assert.NotNull(basket.Items.FirstOrDefault(x => x.ProductId == onOutletProduct.Id));

            await _apiFixture.BuyerWebClient.PostAsync<CheckoutBasketRequestModel, BaseResponseModel>(ApiEndpoints.OrderCheckoutApiEndpoint, new CheckoutBasketRequestModel
            {
                BasketId = basket.Id,
                ClientId = Clients.Id,
                ClientName = Clients.Name
            });

            int timeoutInSeconds = 30;
            int elapsedSeconds = 0;
            PagedResults<IEnumerable<Order>> getResults = null;

            while (elapsedSeconds < timeoutInSeconds)
            {
                getResults = await _apiFixture.BuyerWebClient.GetAsync<PagedResults<IEnumerable<Order>>>($"{ApiEndpoints.GetOrdersApiEndpoint}?pageIndex={Constants.DefaultPageIndex}&itemsPerPage={Constants.DefaultItemsPerPage}");

                if (getResults.Data.Any(x => x.OrderItems.Any(y => y.ProductId == onStockProduct.Id || y.ProductId == onOutletProduct.Id)))
                {
                    break;
                }

                await Task.Delay(1000);
                elapsedSeconds++;
            }

            var order = getResults.Data.FirstOrDefault(x => x.OrderItems.Any(x => x.ProductId == onStockProduct.Id || x.ProductId == onOutletProduct.Id));

            Assert.NotNull(order);
            Assert.NotEmpty(order.OrderItems);

            var stockOrderItem = order.OrderItems.FirstOrDefault(x => x.ProductId == onStockProduct.Id);

            Assert.NotNull(stockOrderItem);
            Assert.Equal(Products.Anton.AvailableQuantity, stockOrderItem.StockQuantity);

            var outletOrderItem = order.OrderItems.FirstOrDefault(x => x.ProductId == onOutletProduct.Id);

            Assert.NotNull(outletOrderItem);
            Assert.Equal(Products.Aga.AvailableQuantity, outletOrderItem.OutletQuantity);
        }
    }
}
