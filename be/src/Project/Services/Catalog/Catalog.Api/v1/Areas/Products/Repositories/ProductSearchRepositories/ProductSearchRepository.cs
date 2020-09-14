using Catalog.Api.Infrastructure.Products.Entities;
using Catalog.Api.v1.Areas.Products.SearchResultModels;
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

        public async Task<PagedResults<IEnumerable<ProductSearchResultModel>>> GetAsync(string language, Guid? categoryId, Guid? brandId, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var query = Query<ProductSearchResultModel>.Term(x => x.IsActive && x.CategoryId == categoryId && x.Language == language, true);

            if (categoryId.HasValue)
            {
                query = query && Query<ProductSearchResultModel>.Term(x => x.CategoryId == categoryId.Value, true);
            }

            if (brandId.HasValue)
            {
                query = query && Query<ProductSearchResultModel>.Term(x => x.BrandId == brandId.Value, true);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query && Query<ProductSearchResultModel>.QueryString(d => d.Query(searchTerm));
            }

            var response = await this.elasticClient.SearchAsync<ProductSearchResultModel>(s => s.From((pageIndex - 1) * itemsPerPage).Size(itemsPerPage).Query(x => x && query));

            if (response.IsValid && response.Hits.Any())
            {
                return new PagedResults<IEnumerable<ProductSearchResultModel>>(response.Total, itemsPerPage)
                {
                    Data = response.Documents
                };
            }

            return default;
        }
    }
}
