using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.Repositories.Products.ProductIndexingRepositories;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.BackgroundTasks.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly CatalogContext _context;
        private readonly IProductIndexingRepository _productIndexingRepository;
        private readonly ILogger<ProductsService> _logger;

        public ProductsService(
            CatalogContext context,
            IProductIndexingRepository productIndexingRepository,
            ILogger<ProductsService> logger)
        {
            _context = context;
            _productIndexingRepository = productIndexingRepository;
            _logger = logger;
        }

        public async Task IndexAllAsync(Guid? sellerId)
        {
            if (sellerId.HasValue)
            {
                foreach (var productId in _context.Products.Where(x => x.Brand.SellerId == sellerId).Select(x => x.Id).ToList())
                {
                    try
                    {
                        await _productIndexingRepository.IndexAsync(productId);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception, $"Couldn't index product: {productId}");
                    }
                }
            }
        }

        public async Task IndexCategoryProducts(Guid? categoryId, Guid? sellerId)
        {
            if (sellerId.HasValue)
            {
                foreach (var productId in _context.Products.Where(x => x.Brand.SellerId == sellerId && x.Category.Id == categoryId).Select(x => x.Id).ToList())
                {
                    try
                    {
                        await _productIndexingRepository.IndexAsync(productId);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception, $"Couldn't index product: {productId}");
                    }
                }
            }
        }
    }
}
