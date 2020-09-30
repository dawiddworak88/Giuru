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

        public async Task<PagedResults<IEnumerable<ProductSearchModel>>> GetAsync(string language, Guid? categoryId, Guid? brandId, string searchTerm, int pageIndex, int itemsPerPage, bool primaryProductsOnly, bool productVariantsOnly)
        {
            var query = Query<ProductSearchModel>.Match(m => m.Field(f => f.Language).Query(language))
                && Query<ProductSearchModel>.Term(t => t.IsActive, true);

            if (categoryId.HasValue)
            {
                query = query && Query<ProductSearchModel>.Match(m => m.Field(f => f.CategoryId).Query(categoryId.Value.ToString()));
            }

            if (brandId.HasValue)
            {
                query = query && Query<ProductSearchModel>.Match(m => m.Field(f => f.BrandId).Query(brandId.Value.ToString()));
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query && Query<ProductSearchModel>.QueryString(d => d.Query(searchTerm));
            }

            if (primaryProductsOnly)
            {
                query = query && Query<ProductSearchModel>.Term(t => t.PrimaryProductIdHasValue, false);
            }

            if (productVariantsOnly)
            {
                query = query && Query<ProductSearchModel>.Term(t => t.PrimaryProductIdHasValue, true);
            }

            var response = await this.elasticClient.SearchAsync<ProductSearchModel>(s => s.From((pageIndex - 1) * itemsPerPage).Size(itemsPerPage).Query(x => x && query).Sort(s => s.Descending(SortSpecialField.Score).Descending(f => f.CreatedDate)));

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
