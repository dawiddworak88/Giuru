using Catalog.Api.v1.Areas.Products.Models;
using Catalog.Api.v1.Areas.Products.ResultModels;
using Foundation.GenericRepository.Services;
using System.Threading.Tasks;
using Catalog.Api.Infrastructure;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using Catalog.Api.Infrastructure.Products.Entities;
using Nest;
using System.Linq;

namespace Catalog.Api.v1.Areas.Products.Services
{
    public class ProductService : IProductService
    {
        private readonly IElasticClient elasticClient;
        private readonly CatalogContext context;
        private readonly IEntityService entityService;

        public ProductService(
            IElasticClient elasticClient,
            CatalogContext context,
            IEntityService entityService)
        {
            this.elasticClient = elasticClient;
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

        public async Task<PagedResults<IEnumerable<Product>>> GetAsync(GetProductsModel getProductsModel)
        {
            var query = Query<Product>.Term(x => x.IsActive, true);

            if (!string.IsNullOrWhiteSpace(getProductsModel.SearchTerm))
            {
                query = query && Query<Product>.QueryString(d => d.Query(getProductsModel.SearchTerm));
            }

            var response = await this.elasticClient.SearchAsync<Product>(s => s.From((getProductsModel.PageIndex - 1) * getProductsModel.ItemsPerPage).Size(getProductsModel.ItemsPerPage).Query(x => x && query));

            if (response.IsValid && response.Hits.Any())
            {
                return new PagedResults<IEnumerable<Product>>(response.Total, getProductsModel.ItemsPerPage)
                { 
                    Data = response.Documents
                };
            }

            return default;
        }

        public async Task<ProductResultModel> GetByIdAsync(GetProductModel getProductModel)
        {
            return default;
        }
    }
}
