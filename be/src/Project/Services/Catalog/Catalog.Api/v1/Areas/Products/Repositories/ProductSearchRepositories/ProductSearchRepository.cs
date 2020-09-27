using Catalog.Api.Infrastructure.Products.Entities;
using Catalog.Api.v1.Areas.Products.SearchModels;
using Foundation.GenericRepository.Paginations;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Products.Repositories.ProductSearchRepositories
{
    public class ProductSearchRepository : IProductSearchRepository
    {
        private readonly IElasticClient elasticClient;

        public ProductSearchRepository(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public async Task<PagedResults<IEnumerable<ProductSearchModel>>> GetAsync(string language, Guid? categoryId, Guid? brandId, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var query = Query<ProductSearchModel>.Term(x => x.IsActive && x.CategoryId == categoryId && x.Language == language, true);

            if (categoryId.HasValue)
            {
                query = query && Query<ProductSearchModel>.Term(x => x.CategoryId == categoryId.Value, true);
            }

            if (brandId.HasValue)
            {
                query = query && Query<ProductSearchModel>.Term(x => x.BrandId == brandId.Value, true);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query && Query<ProductSearchModel>.QueryString(d => d.Query(searchTerm));
            }

            var response = await this.elasticClient.SearchAsync<ProductSearchModel>(s => s.From((pageIndex - 1) * itemsPerPage).Size(itemsPerPage).Query(x => x && query));

            if (response.IsValid && response.Hits.Any())
            {
                return new PagedResults<IEnumerable<ProductSearchModel>>(response.Total, itemsPerPage)
                {
                    Data = response.Documents
                };
            }

            return default;
        }
    }
}
