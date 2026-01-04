using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Paginations;
using Giuru.IntegrationTests.Definitions;
using Seller.Web.Areas.Products.DomainModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

            Console.WriteLine($"Product {productId} did not become visible after {stopwatch.Elapsed.TotalSeconds:F2}s and {attemptCount} attempts");
            Console.WriteLine($"Attempting direct Elasticsearch check as fallback...");
            
            var existsInElasticsearch = await CheckProductInElasticsearchAsync(apiFixture.ElasticsearchUrl, productId);
            
            if (existsInElasticsearch)
            {
                Console.WriteLine($"Product {productId} EXISTS in Elasticsearch but not visible via API - possible API/caching issue");
            }
            else
            {
                Console.WriteLine($"Product {productId} NOT FOUND in Elasticsearch - indexing failed");
            }
            
            return false;
        }

        private static async Task<bool> CheckProductInElasticsearchAsync(string elasticsearchUrl, Guid productId)
        {
            try
            {
                using var httpClient = new HttpClient();
                var searchQuery = new
                {
                    query = new
                    {
                        term = new
                        {
                            productId = productId.ToString()
                        }
                    }
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(searchQuery),
                    Encoding.UTF8,
                    "application/json");

                var response = await httpClient.PostAsync(
                    $"{elasticsearchUrl}/catalog/_search",
                    content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var jsonDoc = JsonDocument.Parse(responseBody);
                    var hits = jsonDoc.RootElement.GetProperty("hits").GetProperty("total");
                    
                    var totalHits = hits.TryGetProperty("value", out var valueElement) 
                        ? valueElement.GetInt32() 
                        : hits.GetInt32();

                    Console.WriteLine($"Elasticsearch direct check: found {totalHits} documents for product {productId}");
                    return totalHits > 0;
                }
                
                Console.WriteLine($"Elasticsearch check failed with status: {response.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Elasticsearch direct check failed: {ex.Message}");
                return false;
            }
        }
    }
}
