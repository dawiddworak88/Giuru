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
        private readonly CatalogContext catalogContext;
        private readonly IProductIndexingRepository productIndexingRepository;
        private readonly ILogger logger;

        public ProductsService(
            CatalogContext catalogContext,
            IProductIndexingRepository productIndexingRepository,
            ILogger<ProductsService> logger)
        {
            this.catalogContext = catalogContext;
            this.productIndexingRepository = productIndexingRepository;
            this.logger = logger;
        }

        public async Task IndexAllAsync(Guid? sellerId)
        {
            if (sellerId.HasValue)
            {
                try
                {
                    await this.productIndexingRepository.DeleteAsync(sellerId.Value);
                }
                catch (Exception exception)
                {
                    this.logger.LogError(exception, $"Couldn't delete products on index all for seller: {sellerId.Value}");
                }

                foreach (var productId in this.catalogContext.Products.Where(x => x.Brand.SellerId == sellerId).Select(x => x.Id).ToList())
                {
                    try
                    {
                        await this.productIndexingRepository.IndexAsync(productId);
                    }
                    catch (Exception exception)
                    {
                        this.logger.LogError(exception, $"Couldn't index product: {productId}");
                    }
                }
            }
        }
    }
}
