using Catalog.Api.v1.Areas.Products.Models;
using Catalog.Api.v1.Areas.Products.ResultModels;
using Foundation.GenericRepository.Services;
using System.Threading.Tasks;
using Catalog.Api.Infrastructure;

namespace Catalog.Api.v1.Areas.Products.Services
{
    public class ProductService : IProductService
    {
        private readonly CatalogContext context;
        private readonly IEntityService entityService;

        public ProductService(
            CatalogContext context,
            IEntityService entityService)
        {
            this.context = context;
            this.entityService = entityService;
        }

        public async Task<ProductResultModel> CreateAsync(CreateUpdateProductModel model)
        {
            return default;
        }

        public async Task<ProductResultModel> UpdateAsync(CreateUpdateProductModel model)
        {
            return default;
        }

        public async Task<DeleteProductResultModel> DeleteAsync(DeleteProductModel deleteProductModel)
        {
            return default;
        }

        public async Task<ProductsResultModel> GetAsync(GetProductsModel getProductsModel)
        {
            return default;
        }

        public async Task<ProductResultModel> GetByIdAsync(GetProductModel getProductModel)
        {
            return default;
        }
    }
}
