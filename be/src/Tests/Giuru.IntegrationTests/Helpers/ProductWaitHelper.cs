using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Paginations;
using Giuru.IntegrationTests.Definitions;
using Seller.Web.Areas.Products.DomainModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests.Helpers
{
    public static class ProductWaitHelper
    {
        public static async Task<bool> WaitForProductToBeVisibleAsync(
            ApiFixture apiFixture,
            Guid productId,
            int timeoutInSeconds = LoopTiming.TimeoutInSeconds,
            int initialDelayInMilliseconds = LoopTiming.InitialDelayInMilliseconds)
        {
            var stopwatch = Stopwatch.StartNew();
            var currentDelay = initialDelayInMilliseconds;
            var attemptCount = 0;

            while (stopwatch.Elapsed.TotalSeconds < timeoutInSeconds)
            {
                attemptCount++;
                
                try
                {
                    var products = await apiFixture.SellerWebClient.GetAsync<PagedResults<IEnumerable<Product>>>(
                        $"{ApiEndpoints.GetProductsApiEndpoint}?pageIndex={Constants.DefaultPageIndex}&itemsPerPage={Constants.DefaultItemsPerPage}");

                    if (products?.Data != null && products.Data.Any(p => p.Id == productId))
                    {
                        Console.WriteLine($"Product {productId} became visible after {stopwatch.Elapsed.TotalSeconds:F2}s and {attemptCount} attempts");
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Attempt {attemptCount} failed: {ex.Message}");
                }

                await Task.Delay(currentDelay);
                
                currentDelay = Math.Min(currentDelay * 2, LoopTiming.MaxDelayInMilliseconds);
            }
            
            return false;
        }
    }
}
