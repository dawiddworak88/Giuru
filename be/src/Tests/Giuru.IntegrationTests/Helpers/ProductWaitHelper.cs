using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Paginations;
using Giuru.IntegrationTests.Definitions;
using Seller.Web.Areas.Products.DomainModels;
using System;
using System.Collections.Generic;
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
            int delayInMilliseconds = LoopTiming.Delay)
        {
            int elapsedSeconds = 0;

            while (elapsedSeconds < timeoutInSeconds)
            {
                try
                {
                    var products = await apiFixture.SellerWebClient.GetAsync<PagedResults<IEnumerable<Product>>>(
                        $"{ApiEndpoints.GetProductsApiEndpoint}?pageIndex={Constants.DefaultPageIndex}&itemsPerPage={Constants.DefaultItemsPerPage}");

                    if (products?.Data != null && products.Data.Any(p => p.Id == productId))
                    {
                        return true;
                    }
                }
                catch
                {
                }

                await Task.Delay(delayInMilliseconds);
                elapsedSeconds++;
            }

            return false;
        }
    }
}
