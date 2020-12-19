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

        public async Task<PagedResults<IEnumerable<ProductSearchModel>>> GetAsync(string language, Guid? categoryId, Guid? sellerId, bool includeProductVariants, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var query = Query<ProductSearchModel>.Term(t => t.Language, language)
                && Query<ProductSearchModel>.Term(t => t.IsActive, true);

            if (categoryId.HasValue)
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.CategoryId).Value(categoryId.Value));
            }

            if (sellerId.HasValue)
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.SellerId).Value(sellerId.Value));
            }

            if (!includeProductVariants)
            {
                query = query && Query<ProductSearchModel>.Term(t => t.PrimaryProductIdHasValue, false);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query && 
                    (Query<ProductSearchModel>.QueryString(d => d.Query(searchTerm))
                        || Query<ProductSearchModel>.Match(x => x.Field(f => f.CategoryName).Query(searchTerm).Fuzziness(Fuzziness.Auto))
                        || Query<ProductSearchModel>.Prefix(x => x.Name.Suffix("keyword"), searchTerm));
            }

            var response = await this.elasticClient.SearchAsync<ProductSearchModel>(s => s.From((pageIndex - 1) * itemsPerPage).Size(itemsPerPage).Query(x => x && query).Sort(s => s.Descending(SortSpecialField.Score).Ascending(x => x.CreatedDate)));

            if (response.IsValid)
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
            var query = Query<ProductSearchModel>.Term(t => t.Field(x => x.ProductId).Value(id))
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
                idsQuery = idsQuery || Query<ProductSearchModel>.Term(t => t.Field(x => x.ProductId).Value(id));
            }

            query = query && idsQuery;

            var response = await this.elasticClient.SearchAsync<ProductSearchModel>(s => s.Query(x => x && query).Sort(s => s.Ascending(f => f.CreatedDate)));

            if (response.IsValid && response.Hits.Any())
            {
                return new PagedResults<IEnumerable<ProductSearchModel>>(response.Total, (int)response.Total)
                {
                    Data = response.Documents
                };
            }

            return default;
        }

        public async Task<int?> CountAllAsync()
        {
            var query = Query<ProductSearchModel>.Term(t => t.IsActive, true);

            var response = await this.elasticClient.SearchAsync<ProductSearchModel>(s => s.Query(x => x && query));

            if (response.IsValid)
            {
                return response.Hits.Count;
            }

            return default;
        }

        public async Task<PagedResults<IEnumerable<ProductSearchModel>>> GetProductVariantsAsync(Guid id, string language)
        {
            var query = Query<ProductSearchModel>.Term(t => t.Field(x => x.PrimaryProductId).Value(id))
                && Query<ProductSearchModel>.Term(t => t.Language, language)
                && Query<ProductSearchModel>.Term(t => t.IsActive, true);

            var response = await this.elasticClient.SearchAsync<ProductSearchModel>(s => s.Query(x => x && query).Sort(s => s.Ascending(x => x.CreatedDate)));

            if (response.IsValid && response.Hits.Any())
            {
                return new PagedResults<IEnumerable<ProductSearchModel>>(response.Total, (int)response.Total)
                {
                    Data = response.Documents
                };
            }

            return default;
        }

        public IEnumerable<string> GetProductSuggestions(string searchTerm, int size)
        {
            var suggestions = new List<string>();

            var categoryResponse = this.elasticClient.Search<ProductSearchModel>(s => s
                .Suggest(su => su
                    .Completion("categoryName", cs => cs
                        .Contexts(ctx => ctx
                            .Context("isActive", x => x.Context(true.ToString())))
                        .Field(f => f.CategoryNameSuggest)
                        .Prefix(searchTerm)
                        .Fuzzy(f => f
                            .Fuzziness(Fuzziness.Auto)
                        )
                    )
                )
            );

            var categorySuggestions =
                from suggest in categoryResponse.Suggest["categoryName"]
                from option in suggest.Options
                select option.Text;

            var nameResponse = this.elasticClient.Search<ProductSearchModel>(s => s
                .Suggest(su => su
                    .Completion("name", cs => cs
                        .Contexts(ctx => ctx
                            .Context("isActive", x => x.Context(true.ToString())))
                        .Contexts(ctx => ctx
                            .Context("primaryProductIdHasValue", x => x.Context(false.ToString())))
                        .Field(f => f.NameSuggest)
                        .Prefix(searchTerm)
                        .Fuzzy(f => f
                            .Fuzziness(Fuzziness.Auto)
                        )
                    )
                )
            );

            var nameSuggestions = 
                from suggest in nameResponse.Suggest["name"]
                from option in suggest.Options
                select option.Text;

            var brandResponse = this.elasticClient.Search<ProductSearchModel>(s => s
                .Suggest(su => su
                    .Completion("brandName", cs => cs
                        .Contexts(ctx => ctx
                            .Context("isActive", x => x.Context(true.ToString())))
                        .Contexts(ctx => ctx
                            .Context("primaryProductIdHasValue", x => x.Context(false.ToString())))
                        .Field(f => f.BrandNameSuggest)
                        .Prefix(searchTerm)
                        .Fuzzy(f => f
                            .Fuzziness(Fuzziness.Auto)
                        )
                    )
                )
            );

            var brandSuggestions =
                from suggest in brandResponse.Suggest["brandName"]
                from option in suggest.Options
                select option.Text;

            suggestions.AddRange(categorySuggestions.Distinct());
            suggestions.AddRange(nameSuggestions.Distinct());
            suggestions.AddRange(brandSuggestions.Distinct());

            return suggestions.Take(size);
        }
    }
}
