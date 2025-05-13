using Catalog.Api.v1.Products.RequestModels;
using Foundation.ApiExtensions.Models.Response;
using Giuru.IntegrationTests.Definitions;
using Inventory.Api.v1.RequestModels;
using System;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests.Helpers
{
    public static class InventoryHelper
    {
        public static async Task<Guid?> CreateProductAndAddToStockAsync(ApiFixture apiFixture, ProductRequestModel product, string endpoint = null)
        {
            var newProduct = await apiFixture.SellerWebClient.PostAsync<ProductRequestModel, BaseResponseModel>(ApiEndpoints.ProductsApiEndpoint, product);

            Assert.NotNull(newProduct);
            Assert.NotEqual(Guid.Empty, newProduct.Id);

            if (endpoint is not null)
            {
                var addToStock = await apiFixture.SellerWebClient.PostAsync<InventoryRequestModel, BaseResponseModel>(endpoint, new InventoryRequestModel
                {
                    ProductId = newProduct.Id,
                    WarehouseId = Inventories.WarehouseId,
                    AvailableQuantity = Inventories.Quantities.AvailableQuantity,
                    Quantity = Inventories.Quantities.Quantity,
                });

                Assert.NotNull(addToStock);
                Assert.NotEqual(Guid.Empty, addToStock.Id);
            }

            return newProduct.Id;
        }
    }
}
