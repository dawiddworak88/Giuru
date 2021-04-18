using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.Repositories.Products.ProductIndexingRepositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.BackgroundTasks.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly CatalogContext catalogContext;
        private readonly IProductIndexingRepository productIndexingRepository;

        public ProductsService(
            CatalogContext catalogContext,
            IProductIndexingRepository productIndexingRepository)
        {
            this.catalogContext = catalogContext;
            this.productIndexingRepository = productIndexingRepository;
        }

        public async Task IndexAllAsync(Guid? sellerId)
        {
            if (sellerId.HasValue)
            {
                await this.productIndexingRepository.DeleteAsync(sellerId.Value);

                foreach (var productId in this.catalogContext.Products.Where(x => x.Brand.SellerId == sellerId).Select(x => x.Id).ToList())
                {
                    await this.productIndexingRepository.IndexAsync(productId);
                }
            }
        }
    }
}
