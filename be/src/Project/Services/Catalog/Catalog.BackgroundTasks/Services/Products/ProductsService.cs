using Catalog.BackgroundTasks.ServicesModels;
using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.Repositories.ProductIndexingRepositories;
using Foundation.Catalog.Repositories.ProductSearchRepositories;
using Foundation.Localization.Definitions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly IProductSearchRepository _productSearchRepository;
        private readonly ILogger<ProductsService> _logger;
        private readonly IOptionsMonitor<LocalizationSettings> _localizationSettings;

        public ProductsService(
            CatalogContext context,
            IProductIndexingRepository productIndexingRepository,
            IBulkProductIndexingRepository bulkProductIndexingRepository,
            IProductSearchRepository productSearchRepository,
            ILogger<ProductsService> logger,
            IOptionsMonitor<LocalizationSettings> localizationSettings)
        {
            _context = context;
            _productIndexingRepository = productIndexingRepository;
            _bulkProductIndexingRepository = bulkProductIndexingRepository;
            _productSearchRepository = productSearchRepository;
            _logger = logger;
            _localizationSettings = localizationSettings;
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

        public async Task BatchUpdateStockAvailableQuantitiesAsync(IEnumerable<AvailableQuantityServiceModel> availableQuantities)
        {
            var skus = availableQuantities?.Select(x => x.ProductSku).Distinct().ToList();

            var products = await _productSearchRepository.GetAsync(_localizationSettings.CurrentValue.DefaultCulture, null, skus, null);

            var supportedCultures = _localizationSettings.CurrentValue.SupportedCultures.Split(",");

            foreach (var availableProduct in availableQuantities)
            {
                var product = products.Data.FirstOrDefault(x => x.Sku == availableProduct.ProductSku);

                if (product is null)
                {
                    _logger.LogError($"Product with SKU {availableProduct.ProductSku} not found in search index");
                    continue;
                }

                if (product.StockAvailableQuantity == availableProduct.AvailableQuantity)
                {
                    _logger.LogError($"Product with SKU {availableProduct.ProductSku} has the same stock available quantity, skipping update");
                    continue;
                }

                foreach (var supportedCulture in supportedCultures)
                {
                    var docId = $"{product.ProductId}_{supportedCulture}";

                    await _productIndexingRepository.UpdateStockAvailableQuantity(docId, availableProduct.AvailableQuantity);
                }
            }
        }
    }
}
