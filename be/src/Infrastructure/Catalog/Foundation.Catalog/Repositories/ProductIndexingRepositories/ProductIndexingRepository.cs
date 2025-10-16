using Foundation.Catalog.SearchModels.Products;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Catalog.Repositories.ProductIndexingRepositories
{
    public class ProductIndexingRepository : IProductIndexingRepository
    {
        private readonly IBulkProductIndexingRepository _bulkProductIndexingRepository;
        private readonly IElasticClient _elasticClient;

        public ProductIndexingRepository(
            ILogger<ProductIndexingRepository> logger,
            IBulkProductIndexingRepository bulkProductIndexingRepository,
            IElasticClient elasticClient)
        {
            _bulkProductIndexingRepository = bulkProductIndexingRepository;
            _elasticClient = elasticClient;
        }

        public async Task DeleteAsync(Guid sellerId)
        {
            await _elasticClient.DeleteByQueryAsync<ProductSearchModel>(q => q.Query(z => z.Term(p => p.SellerId, sellerId)));
        }

        public async Task IndexAsync(Guid productId)
        {
            await _bulkProductIndexingRepository.IndexBatchAsync([productId]);
        }

        public async Task BulkUpdateStockAvailableQuantity(IEnumerable<(string docId, double availableQuantity)> updates)
        {
            var bulkDescriptor = new BulkDescriptor();

            foreach (var (docId, availableQuantity) in updates)
            {
                bulkDescriptor.Update<ProductSearchModel, object>(u => u
                    .Id(docId)
                    .Doc(new { StockAvailableQuantity = availableQuantity })
                );
            }

            var response = await _elasticClient.BulkAsync(bulkDescriptor);

            if (response.Errors)
            {
                foreach (var item in response.ItemsWithErrors)
                {
                    _logger.LogError($"Failed to update document Id: {item.Id}: {item.Error?.Reason}");
                }
            }
        }

        public async Task BulkUpdateOutletAvailableQuantity(IEnumerable<(string docId, double availableQuantity)> updates)
        {
            var bulkDescriptor = new BulkDescriptor();

            foreach (var (docId, availableQuantity) in updates)
            {
                bulkDescriptor.Update<ProductSearchModel, object>(u => u
                    .Id(docId)
                    .Doc(new { OutletAvailableQuantity = availableQuantity })
                );
            }

            var response = await _elasticClient.BulkAsync(bulkDescriptor);

            if (response.Errors)
            {
                foreach (var item in response.ItemsWithErrors)
                {
                    _logger.LogError($"Failed to update document Id: {item.Id}: {item.Error?.Reason}");
                }
            }
        }
    }
}
