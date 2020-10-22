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

        public async Task<PagedResults<IEnumerable<ProductSearchModel>>> GetAsync(string language, Guid? categoryId, Guid? sellerId, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var query = Query<ProductSearchModel>.Term(t => t.Language, language)
                && Query<ProductSearchModel>.Term(t => t.IsActive, true)
                && Query<ProductSearchModel>.Term(t => t.PrimaryProductIdHasValue, false);

            if (categoryId.HasValue)
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.CategoryId.Suffix("keyword")).Value(categoryId.Value));
            }

            if (sellerId.HasValue)
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.SellerId.Suffix("keyword")).Value(sellerId.Value));
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query && Query<ProductSearchModel>.QueryString(d => d.Query(searchTerm));
            }

            var response = await this.elasticClient.SearchAsync<ProductSearchModel>(s => s.From((pageIndex - 1) * itemsPerPage).Size(itemsPerPage).Query(x => x && query).Sort(s => s.Descending(SortSpecialField.Score)));

            if (response.IsValid && response.Hits.Any())
            {
                return new PagedResults<IEnumerable<ProductSearchModel>>(response.Total, itemsPerPage)
                {
                    Data = response.Documents
                };
            }

            return default;
        }

        public async Task<ProductSearchModel> GetAsync(Guid id, string language)
        {
            var query = Query<ProductSearchModel>.Term(t => t.Field(x => x.ProductId.Suffix("keyword")).Value(id))
                && Query<ProductSearchModel>.Term(t => t.Language, language)
                && Query<ProductSearchModel>.Term(t => t.IsActive, true);

            var response = await this.elasticClient.SearchAsync<ProductSearchModel>(s => s.Query(x => x && query));

            if (response.IsValid && response.Hits.Any())
            {
                return response.Documents.FirstOrDefault();
            }

            return default;
        }

        public async Task<PagedResults<IEnumerable<ProductSearchModel>>> GetAsync(string language, IEnumerable<Guid> ids)
        {
            var query = Query<ProductSearchModel>.Term(t => t.Language, language)
                && Query<ProductSearchModel>.Term(t => t.IsActive, true);

            var idsQuery = Query<ProductSearchModel>.MatchNone();

            foreach (var id in ids)
            {
                idsQuery = idsQuery || Query<ProductSearchModel>.Term(t => t.Field(x => x.ProductId.Suffix("keyword")).Value(id));
            }

            query = query && idsQuery;

            var response = await this.elasticClient.SearchAsync<ProductSearchModel>(s => s.Query(x => x && query));

            if (response.IsValid && response.Hits.Any())
            {
                return new PagedResults<IEnumerable<ProductSearchModel>>(response.Total, (int)response.Total)
                {
                    Data = response.Documents
                };
            }

            return default;
        }

        public async Task<PagedResults<IEnumerable<ProductSearchModel>>> GetProductVariantsAsync(Guid id, string language)
        {
            var query = Query<ProductSearchModel>.Term(t => t.Field(x => x.PrimaryProductId.Suffix("keyword")).Value(id))
                && Query<ProductSearchModel>.Term(t => t.Language, language)
                && Query<ProductSearchModel>.Term(t => t.IsActive, true);

            var response = await this.elasticClient.SearchAsync<ProductSearchModel>(s => s.Query(x => x && query));

            if (response.IsValid && response.Hits.Any())
            {
                return new PagedResults<IEnumerable<ProductSearchModel>>(response.Total, (int)response.Total)
                {
                    Data = response.Documents
                };
            }

            return default;
        }

        public async Task<IEnumerable<string>> GetProductSuggestionsAsync(string searchTerm, int size, string language)
        {
            var query = Query<ProductSearchModel>.Term(t => t.PrimaryProductIdHasValue, false)
                && Query<ProductSearchModel>.Term(t => t.Language, language)
                && Query<ProductSearchModel>.Term(t => t.IsActive, true);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query && Query<ProductSearchModel>.Match(d => d.Field(f => f.Name).Query(searchTerm));
            }

            var response = await this.elasticClient.SearchAsync<ProductSearchModel>(s => s.Size(size).Query(x => x && query));

            if (response.IsValid && response.Hits.Any())
            {
                return response.Documents.Select(x => x.Name).Distinct();
            }

            return default;
        }
    }
}
