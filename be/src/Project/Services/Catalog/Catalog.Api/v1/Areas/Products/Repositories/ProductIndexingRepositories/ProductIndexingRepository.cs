using Catalog.Api.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Products.Repositories.ProductIndexingRepositories
{
    public class ProductIndexingRepository : IProductIndexingRepository
    {
        private readonly CatalogContext catalogContext;

        public ProductIndexingRepository(CatalogContext catalogContext)
        {
            this.catalogContext = catalogContext;
        }

        public async Task IndexAsync(Guid productId)
        { 
        }
    }
}
