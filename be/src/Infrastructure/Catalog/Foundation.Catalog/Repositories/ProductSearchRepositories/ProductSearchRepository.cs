using Foundation.Catalog.Definitions;
using Foundation.Catalog.SearchModels.Products;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Paginations;
using Foundation.Search.Extensions;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Catalog.Repositories.ProductSearchRepositories
{
    public class ProductSearchRepository : IProductSearchRepository
    {
        private readonly IElasticClient _elasticClient;

        public ProductSearchRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<PagedResults<IEnumerable<ProductSearchModel>>> GetAsync(
            string language, 
            Guid? categoryId, 
            Guid? sellerId, 
            bool? hasPrimaryProduct,
            bool? isNew,
            bool? isSeller,
            string searchTerm, 
            int? pageIndex, 
            int? itemsPerPage,
            string orderBy)
        {
            var query = Query<ProductSearchModel>.Term(t => t.Language, language)
                && Query<ProductSearchModel>.Term(t => t.IsActive, true);

            if (categoryId.HasValue)
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.CategoryId).Value(categoryId.Value));
            }

            if (sellerId.HasValue && isSeller is true)
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.SellerId).Value(sellerId.Value));
            }
            else
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.IsPublished).Value(true));
            }

            if (hasPrimaryProduct.HasValue)
            {
                query = query && Query<ProductSearchModel>.Term(t => t.PrimaryProductIdHasValue, hasPrimaryProduct.Value);
            }

            if (isNew.HasValue)
            {
                query = query && Query<ProductSearchModel>.Term(t => t.IsNew, isNew.Value);
            }
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query && 
                    (Query<ProductSearchModel>.QueryString(d => d.Query(searchTerm))
                        || Query<ProductSearchModel>.Prefix(x => x.Sku, searchTerm)
                        || Query<ProductSearchModel>.Prefix(x => x.Ean, searchTerm)
                        || Query<ProductSearchModel>.Match(x => x.Field(f => f.CategoryName).Query(searchTerm).Fuzziness(Fuzziness.Auto))
                        || Query<ProductSearchModel>.Prefix(x => x.Name.Suffix("keyword"), searchTerm));
            }

            if (pageIndex.HasValue is false || itemsPerPage.HasValue is false)
            {
                pageIndex = Constants.DefaultPageIndex;
                itemsPerPage = Constants.MaxItemsPerPageLimit;
            }

            var response = await _elasticClient.SearchAsync<ProductSearchModel>(q => q.TrackTotalHits().From((pageIndex - 1) * itemsPerPage).Size(itemsPerPage).Query(q => query).Sort(s => Sorting<ProductSearchModel>(orderBy)));

            if (response.IsValid)
            {
                return new PagedResults<IEnumerable<ProductSearchModel>>(response.Total, itemsPerPage.Value)
                {
                    Data = response.Documents
                };
            }

            return default;
        }

        public async Task<ProductSearchModel> GetByIdAsync(Guid id, string language, Guid? organisationId, bool? isSeller)
        {
            var query = Query<ProductSearchModel>.Term(t => t.Field(x => x.ProductId).Value(id))
                && Query<ProductSearchModel>.Term(t => t.Language, language)
                && Query<ProductSearchModel>.Term(t => t.IsActive, true);

            if (organisationId.HasValue && isSeller is true)
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.SellerId).Value(organisationId.Value));
            }
            else
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.IsPublished).Value(true));
            }

            var response = await _elasticClient.SearchAsync<ProductSearchModel>(s => s.Query(x => x && query));

            if (response.IsValid && response.Hits.Any())
            {
                return response.Documents.FirstOrDefault();
            }

            return default;
        }

        public async Task<ProductSearchModel> GetBySkuAsync(string sku, string language, Guid? organisationId, bool? isSeller)
        {
            var query = Query<ProductSearchModel>.Term(t => t.Field(x => x.Sku.Suffix("keyword")).Value(sku))
                && Query<ProductSearchModel>.Term(t => t.Language, language)
                && Query<ProductSearchModel>.Term(t => t.IsActive, true);

            if (organisationId.HasValue && isSeller is true)
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.SellerId).Value(organisationId.Value));
            }
            else
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.IsPublished).Value(true));
            }

            var response = await _elasticClient.SearchAsync<ProductSearchModel>(s => s.Query(x => x && query));

            if (response.IsValid && response.Hits.Any())
            {
                return response.Documents.FirstOrDefault();
            }

            return default;
        }

        public async Task<PagedResults<IEnumerable<ProductSearchModel>>> GetAsync(string language, Guid? organisationId, bool? isSeller, IEnumerable<Guid> ids, string orderBy)
        {
            var query = Query<ProductSearchModel>.Term(t => t.Language, language)
                && Query<ProductSearchModel>.Term(t => t.IsActive, true);

            if (organisationId.HasValue && isSeller is true)
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.SellerId).Value(organisationId.Value));
            }
            else
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.IsPublished).Value(true));
            }

            var idsQuery = Query<ProductSearchModel>.MatchNone();

            foreach (var id in ids)
            {
                idsQuery = idsQuery || Query<ProductSearchModel>.Term(t => t.Field(x => x.ProductId).Value(id));
            }

            query = query && idsQuery;

            var response = await _elasticClient.SearchAsync<ProductSearchModel>(q => q.From(ProductSearchConstants.Pagination.BeginningPage).Size(ProductSearchConstants.Pagination.ProductsMaxSize).Query(q => query).Sort(s => Sorting<ProductSearchModel>(orderBy)));

            if (response.IsValid && response.Hits.Any())
            {
                return new PagedResults<IEnumerable<ProductSearchModel>>(response.Total, (int)response.Total)
                {
                    Data = response.Documents
                };
            }

            return default;
        }

        public async Task<PagedResults<IEnumerable<ProductSearchModel>>> GetAsync(string language, Guid? organisationId, bool? isSeller, IEnumerable<string> skus, string orderBy)
        {
            var query = Query<ProductSearchModel>.Term(t => t.Language, language)
                && Query<ProductSearchModel>.Term(t => t.IsActive, true);

            if (organisationId.HasValue && isSeller is true)
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.SellerId).Value(organisationId.Value));
            }
            else
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.IsPublished).Value(true));
            }

            var skusQuery = Query<ProductSearchModel>.MatchNone();

            foreach (var sku in skus)
            {
                skusQuery = skusQuery || Query<ProductSearchModel>.Term(t => t.Field(x => x.Sku.Suffix("keyword")).Value(sku));
            }

            query = query && skusQuery;

            var response = await _elasticClient.SearchAsync<ProductSearchModel>(q => q.From(ProductSearchConstants.Pagination.BeginningPage).Size(ProductSearchConstants.Pagination.ProductsMaxSize).Query(q => query).Sort(s => Sorting<ProductSearchModel>(orderBy)));

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

            var response = await _elasticClient.SearchAsync<ProductSearchModel>(s => s.Query(x => x && query));

            if (response.IsValid)
            {
                return response.Hits.Count;
            }

            return default;
        }

        public async Task<PagedResults<IEnumerable<ProductSearchModel>>> GetProductVariantsAsync(Guid id, string language, Guid? organisationId, bool? isSeller)
        {
            var query = Query<ProductSearchModel>.Term(t => t.Field(x => x.PrimaryProductId).Value(id))
                && Query<ProductSearchModel>.Term(t => t.Language, language)
                && Query<ProductSearchModel>.Term(t => t.IsActive, true);

            if (organisationId.HasValue && isSeller is true)
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.SellerId).Value(organisationId.Value));
            }
            else
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.IsPublished).Value(true));
            }

            var searchRequest = new SearchRequest
            {
                From = ProductSearchConstants.Pagination.BeginningPage,
                Size = ProductSearchConstants.Pagination.ProductsMaxSize,
                Query = query
            };

            var response = await _elasticClient.SearchAsync<ProductSearchModel>(searchRequest);

            if (response.IsValid && response.Hits.Any())
            {
                return new PagedResults<IEnumerable<ProductSearchModel>>(response.Total, (int)response.Total)
                {
                    Data = response.Documents
                };
            }

            return default;
        }

        public IEnumerable<string> GetProductSuggestions(string searchTerm, int size, string language, Guid? organisationId)
        {
            var nameResponse = _elasticClient.Search<ProductSearchModel>(s => s
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

            return nameSuggestions.Distinct().Take(size);
        }

        private SortDescriptor<T> Sorting<T>(string orderBy) where T : ProductSearchModel
        {
            if (string.IsNullOrWhiteSpace(orderBy) || 
                orderBy == SortingConstants.Name ||
                orderBy == SortingConstants.Default)
            {
                return new SortDescriptor<T>().Field(f => f.Name.Suffix("keyword"), SortOrder.Ascending);
            }

            if (orderBy == SortingConstants.Newest)
            {
                return new SortDescriptor<T>().Field(f => f.CreatedDate, SortOrder.Descending);
            }

            return orderBy.ToElasticSortList<T>();
        }
    }
}