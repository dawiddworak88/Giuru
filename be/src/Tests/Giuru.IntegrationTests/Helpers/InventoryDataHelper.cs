using Catalog.Api.v1.Products.RequestModels;
using Foundation.ApiExtensions.Models.Response;
using Foundation.GenericRepository.Paginations;
using Giuru.IntegrationTests.Definitions;
using Inventory.Api.v1.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests.Helpers
{
    public static class InventoryDataHelper
    {
        public static async Task<Guid?> CreateProductAndAddToStockAsync(ApiFixture apiFixture, ProductRequestModel product, string endpoint = null)
        {
            var newProdct = await apiFixture.SellerWebClient.PostAsync<ProductRequestModel, BaseResponseModel>(ApiEndpoints.ProductsApiEndpoint, product);

            Assert.NotNull(newProdct);
            Assert.NotEqual(Guid.Empty, newProdct.Id);

            if (endpoint is not null)
            {
                var addToStock = await apiFixture.SellerWebClient.PostAsync<InventoryRequestModel, BaseResponseModel>(endpoint, new InventoryRequestModel
                {
                    ProductId = newProdct.Id,
                    WarehouseId = Inventories.WarehouseId,
                    AvailableQuantity = Inventories.Quantities.AvailableQuantity,
                    Quantity = Inventories.Quantities.Quantity,
                });

                Assert.NotNull(addToStock);
                Assert.NotEqual(Guid.Empty, addToStock.Id);
            }

            return newProdct.Id;
        }

        public static async Task<PagedResults<IEnumerable<T>>> GetDataAsync<T>(
            Func<Task<PagedResults<IEnumerable<T>>>> fetchData,
            Func<T, bool> condition)
        {
            int timeoutInSeconds = LoopTiming.TimeoutInSeconds;
            int elapsedSeconds = LoopTiming.ElapsedSeconds;

            while (elapsedSeconds < timeoutInSeconds)
            {
                var getResults = await fetchData();

                if (getResults.Data.Any(condition))
                {
                    return getResults;
                }

                await Task.Delay(LoopTiming.Delay);
                elapsedSeconds++;
            }

            return null;
        }
    }
}
