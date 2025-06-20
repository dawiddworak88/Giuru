using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.Repositories.ProductIndexingRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.BackgroundTasks.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly CatalogContext _context;
        private readonly IProductIndexingRepository _productIndexingRepository;
        private readonly IBulkProductIndexingRepository _bulkProductIndexingRepository;
        private readonly ILogger<ProductsService> _logger;

        public ProductsService(
            CatalogContext context,
            IProductIndexingRepository productIndexingRepository,
            IBulkProductIndexingRepository bulkProductIndexingRepository,
            ILogger<ProductsService> logger)
        {
            _context = context;
            _productIndexingRepository = productIndexingRepository;
            _bulkProductIndexingRepository = bulkProductIndexingRepository;
            _logger = logger;
        }

        public async Task IndexAllAsync(Guid? sellerId)
        {
            if (!sellerId.HasValue)
                return;

            _logger.LogInformation("Starting bulk indexing for seller: {SellerId}", sellerId);

            try
            {
                var productIds = await GetProductIdsAsync(sellerId.Value);
                await _bulkProductIndexingRepository.IndexBatchAsync(productIds);
                
                _logger.LogInformation("Completed bulk indexing for seller: {SellerId}", sellerId);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to index products for seller: {SellerId}", sellerId);
                throw;
            }
        }

        public async Task IndexCategoryProducts(Guid? categoryId, Guid? sellerId)
        {
            if (!sellerId.HasValue || !categoryId.HasValue)
                return;

            _logger.LogInformation("Starting bulk indexing for category: {CategoryId}, seller: {SellerId}", categoryId, sellerId);

            try
            {
                var productIds = await GetCategoryProductIdsAsync(categoryId.Value, sellerId.Value);
                await _bulkProductIndexingRepository.IndexBatchAsync(productIds);
                
                _logger.LogInformation("Completed bulk indexing for category: {CategoryId}, seller: {SellerId}", categoryId, sellerId);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to index products for category: {CategoryId}, seller: {SellerId}", categoryId, sellerId);
                throw;
            }
        }

        private async Task<List<Guid>> GetProductIdsAsync(Guid sellerId)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(x => x.Brand.SellerId == sellerId)
                .Select(x => x.Id)
                .ToListAsync();
        }

        private async Task<List<Guid>> GetCategoryProductIdsAsync(Guid categoryId, Guid sellerId)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(x => x.Brand.SellerId == sellerId && x.CategoryId == categoryId)
                .Select(x => x.Id)
                .ToListAsync();
        }
    }
}
