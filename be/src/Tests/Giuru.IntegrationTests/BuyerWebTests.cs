using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using Buyer.Web.Areas.Orders.DomainModels;
using Catalog.Api.v1.Products.RequestModels;
using Foundation.ApiExtensions.Models.Response;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Paginations;
using Giuru.IntegrationTests.Definitions;
using Giuru.IntegrationTests.Helpers;
using Seller.Web.Areas.Inventory.ApiRequestModels;
using Seller.Web.Areas.Inventory.DomainModels;
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

        private static async Task<Guid> EnsureWarehouseExistsAsync(ApiFixture apiFixture)
        {
            var existing = await apiFixture.SellerWebClient.GetAsync<PagedResults<IEnumerable<Warehouse>>>(
                $"{ApiEndpoints.GetInventoryWarehouseApiEndpoint}?pageIndex={Constants.DefaultPageIndex}&itemsPerPage={Constants.DefaultItemsPerPage}");

            var warehouse = existing?.Data?.FirstOrDefault(x => x.Name == "Integration Test Warehouse");

            if (warehouse is not null && warehouse.Id != Guid.Empty)
            {
                return warehouse.Id;
            }


            var created = await apiFixture.SellerWebClient.PostAsync<SaveWarehouseRequestModel, BaseResponseModel>(
                ApiEndpoints.InventoryWarehousesApiEndpoint,
                new SaveWarehouseRequestModel
                {
                    Name = "Integration Test Warehouse",
                    Location = "Integration Test"
                });

            Assert.NotNull(created);
            Assert.NotEqual(Guid.Empty, created.Id);

            return created.Id.Value;
        }

        [Fact] 
        public async Task AddToBasket_WithNormalStockAndOutletProducts_CheckoutOrder_Returns_Orders()
        {
            var warehouseId = await EnsureWarehouseExistsAsync(_apiFixture);

            var stockProduct = await InventoryHelper.CreateProductAndAddToStockAsync(_apiFixture, 
                    new ProductRequestModel
                    {
                        Sku = Products.Anton.Sku,
                        Name = Products.Anton.Name,
                        CategoryId = Products.Anton.CategoryId,
                        IsPublished = Products.Anton.IsPublished,
                        Ean = Products.Anton.Ean
                    },
                    warehouseId,
                    ApiEndpoints.InventoriesApiEndpoint);

            var outletProduct = await InventoryHelper.CreateProductAndAddToStockAsync(_apiFixture, 
                    new ProductRequestModel
                    {
                        Sku = Products.Aga.Sku,
                        Name = Products.Aga.Name,
                        CategoryId = Products.Aga.CategoryId,
                        IsPublished = Products.Aga.IsPublished,
                        Ean = Products.Aga.Ean
                    },
                    warehouseId,
                    ApiEndpoints.OutletsApiEndpoint);

            var basket = await _apiFixture.BuyerWebClient.PostAsync<SaveBasketRequestModel, BasketResponseModel>(
                ApiEndpoints.BasketApiEndpoint, 
                new SaveBasketRequestModel
                {
                    Items = new List<BasketItemRequestModel>()
                    {
                        new BasketItemRequestModel
                        {
                            ProductId = Products.Lamica.Id,
                            Sku = Products.Lamica.Sku,
                            Name = Products.Lamica.Name,
                            Quantity = Inventories.Quantities.Quantity,
                        },
                        new BasketItemRequestModel
                        {
                            ProductId = stockProduct,
                            Sku = Products.Anton.Sku,
                            Name = Products.Anton.Name,
                            StockQuantity = Inventories.Quantities.AvailableQuantity
                        },
                        new BasketItemRequestModel
                        {
                            ProductId = outletProduct,
                            Sku = Products.Aga.Sku,
                            Name = Products.Aga.Name,
                            OutletQuantity = Inventories.Quantities.AvailableQuantity
                        }
                    }
                });

            Assert.NotNull(basket);
            Assert.NotEqual(Guid.Empty, basket.Id);
            Assert.NotNull(basket.Items);
            Assert.Equal(Products.Lamica.Id, basket.Items.FirstOrDefault().ProductId);

            await _apiFixture.BuyerWebClient.PostAsync<CheckoutBasketRequestModel, BaseResponseModel>(
                ApiEndpoints.OrderCheckoutApiEndpoint, 
                new CheckoutBasketRequestModel
                {
                    BasketId = basket.Id,
                    ClientId = Clients.Id,
                    ClientName = Clients.Name
                });

            var orders = await _apiFixture.BuyerWebClient.GetAsync<PagedResults<IEnumerable<Order>>>($"{ApiEndpoints.GetOrdersApiEndpoint}?pageIndex={Constants.DefaultPageIndex}&itemsPerPage={Constants.DefaultItemsPerPage}");

            Assert.NotNull(orders.Data);
            Assert.Equal(1, orders.Total);

            var order = orders.Data.FirstOrDefault();

            Assert.Equal(Products.Lamica.Id, order.OrderItems.FirstOrDefault().ProductId);

            var stockOrderItem = order.OrderItems.FirstOrDefault(x => x.ProductId == stockProduct);

            Assert.NotNull(stockOrderItem);
            Assert.Equal(Inventories.Quantities.AvailableQuantity, stockOrderItem.StockQuantity);

            var outletOrderItem = order.OrderItems.FirstOrDefault(x => x.ProductId == outletProduct);

            Assert.NotNull(outletOrderItem);
            Assert.Equal(Inventories.Quantities.AvailableQuantity, outletOrderItem.OutletQuantity);
        }
    }
}
