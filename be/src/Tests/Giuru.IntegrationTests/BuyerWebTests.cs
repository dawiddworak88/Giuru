using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using Buyer.Web.Areas.Orders.DomainModels;
using Catalog.Api.v1.Products.RequestModels;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Shared.Definitions;
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
        public async Task AddToBasket_WhenConfiguredGrulaIsUnavailable_CheckoutOrder_ReturnsUnpricedOrder()
        {
            var warehouseId = await EnsureWarehouseExistsAsync(_apiFixture);

            // A basket line whose SKU resolves to a catalog product must carry that product's id, or the
            // save is refused with 422. Products.Lamica.Id exists nowhere and SellerWebTests creates LAM_01
            // under another id, so this test prices a product of its own.
            var pricedProductSku = $"GRULA-{Guid.NewGuid():N}"[..20];
            var pricedProduct = await InventoryHelper.CreateProductAndAddToStockAsync(_apiFixture,
                    new ProductRequestModel
                    {
                        Sku = pricedProductSku,
                        Name = $"Unpriced {pricedProductSku}",
                        CategoryId = Products.Lamica.CategoryId,
                        IsPublished = true
                    });

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
                            ProductId = pricedProduct,
                            Sku = pricedProductSku,
                            Name = $"Unpriced {pricedProductSku}",
                            Quantity = Inventories.Quantities.Quantity,
                            UnitPrice = 0.01m,
                            Price = 0.01m,
                            Currency = "FAKE"
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
            Assert.Equal(pricedProduct, basket.Items.FirstOrDefault().ProductId);
            var savedBasketItem = basket.Items.Single(x => x.ProductId == pricedProduct);
            Assert.Null(savedBasketItem.UnitPrice);
            Assert.Null(savedBasketItem.Price);
            Assert.Null(savedBasketItem.Currency);

            // Basket API is the trusted internal boundary used by the BFF. Verify the
            // values were removed before that boundary rather than only hidden in the response.
            var persistedBasket = await _apiFixture.BasketApiClient.GetAsync<BasketApiResponseModel>(
                $"{ApiConstants.Baskets.BasketsApiEndpoint}/{basket.Id}");
            var persistedBasketItem = persistedBasket.Items.Single(x => x.ProductId == pricedProduct);
            Assert.Null(persistedBasketItem.UnitPrice);
            Assert.Null(persistedBasketItem.Price);
            Assert.Null(persistedBasketItem.Currency);

            // Checkout is refused for any Client but the signed-in one, so order as the test user's own Client.
            var client = await ClientHelper.GetOrCreateTestUsersClientAsync(_apiFixture);

            await _apiFixture.BuyerWebClient.PostAsync<CheckoutBasketRequestModel, BaseResponseModel>(
                ApiEndpoints.OrderCheckoutApiEndpoint, 
                new CheckoutBasketRequestModel
                {
                    BasketId = basket.Id,
                    ClientId = client.Id,
                    ClientName = Clients.Name
                });

            var orders = await _apiFixture.BuyerWebClient.GetAsync<PagedResults<IEnumerable<Order>>>($"{ApiEndpoints.GetOrdersApiEndpoint}?pageIndex={Constants.DefaultPageIndex}&itemsPerPage={Constants.DefaultItemsPerPage}");

            Assert.NotNull(orders.Data);
            Assert.Equal(1, orders.Total);

            var order = orders.Data.FirstOrDefault();

            Assert.Equal(pricedProduct, order.OrderItems.FirstOrDefault().ProductId);
            var orderItem = order.OrderItems.Single(x => x.ProductId == pricedProduct);
            Assert.Null(orderItem.UnitPrice);
            Assert.Null(orderItem.Price);
            Assert.Null(orderItem.Currency);

            var stockOrderItem = order.OrderItems.FirstOrDefault(x => x.ProductId == stockProduct);

            Assert.NotNull(stockOrderItem);
            Assert.Equal(Inventories.Quantities.AvailableQuantity, stockOrderItem.StockQuantity);

            var outletOrderItem = order.OrderItems.FirstOrDefault(x => x.ProductId == outletProduct);

            Assert.NotNull(outletOrderItem);
            Assert.Equal(Inventories.Quantities.AvailableQuantity, outletOrderItem.OutletQuantity);
        }
    }
}
