using Foundation.Catalog.SearchModels.Products;
using Microsoft.Extensions.Logging;
using Nest;
using System;
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
    }
}
