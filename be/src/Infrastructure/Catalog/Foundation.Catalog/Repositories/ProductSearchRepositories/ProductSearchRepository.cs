using Foundation.Catalog.Definitions;
using Foundation.Catalog.SearchModels.Products;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Paginations;
using Foundation.Search.Extensions;
using Foundation.Search.Models;
using Foundation.Search.Paginations;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Filter = Foundation.Search.Models.Filter;

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

        private List<Filter> MapFilters(ISearchResponse<ProductSearchModel> searchResponse)
        {
            var results = new List<Filter>();

            foreach (var aggregation in searchResponse.Aggregations)
            {
                if (aggregation.Key == "attributes" && aggregation.Value is SingleBucketAggregate nestedAggregation)
                {
                    foreach (var subAggregation in nestedAggregation)
                    {
                        if (subAggregation.Value is BucketAggregate bucketAgg)
                        {
                            var filter = MapBucketAggregate(subAggregation.Key, bucketAgg);

                            if (filter != null)
                                results.Add(filter);
                        }
                    }

                    continue;
                }

                if (aggregation.Value is BucketAggregate bucketAggregate)
                {
                    var filter = MapBucketAggregate(aggregation.Key, bucketAggregate);

                    if (filter != null)
                        results.Add(filter);
                }
            }

            return results;
        }

        public async Task<PagedResultsWithFilters<IEnumerable<ProductSearchModel>>> GetPagedResultsWithFilters(
            string langauge, 
            Guid? organisationId, 
            int? pageIndex, 
            int? itemsPerPage,
            string orderBy,
            QueryFilters filters,
            bool? isSeller)
        {
            var query = Query<ProductSearchModel>.Term(t => t.Language, langauge)
                && Query<ProductSearchModel>.Term(t => t.IsActive, true);

            if (organisationId.HasValue && isSeller is true)
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.SellerId).Value(organisationId.Value));
            }
            else
            {
                query = query && Query<ProductSearchModel>.Term(t => t.Field(x => x.IsPublished).Value(true));
            }

            if (pageIndex.HasValue is false || itemsPerPage.HasValue is false)
            {
                pageIndex = Constants.DefaultPageIndex;
                itemsPerPage = Constants.MaxItemsPerPageLimit;
            }

            var response = await _elasticClient.SearchAsync<ProductSearchModel>(q => q
                .TrackTotalHits()
                .From((pageIndex - 1) * itemsPerPage)
                .Size(itemsPerPage)
                .Query(q => query)
                .PostFilter(fq =>
                    fq.Terms(t => t.Field("categoryName.keyword").Terms(filters.Category)) ||
                    fq.Nested(n => n
                        .Path("productAttributes")
                        .Query(nq => nq.Bool(b => b
                            .Filter(
                                fq => fq.Terms(t => t.Field("productAttributes.primaryColor.value.name.keyword").Terms(filters.Color)),
                                fq => fq.Terms(t => t.Field("productAttributes.shape.value.name.keyword").Terms(filters.Shape)),
                                fq => nq.Bool(b2 => b2
                                    .Should(BuildRangeQueries(filters.Height, "productAttributes.height.value").ToArray())
                                    .MinimumShouldMatch(1)
                                ),
                                fq => nq.Bool(b2 => b2
                                    .Should(BuildRangeQueries(filters.Width, "productAttributes.width.value").ToArray())
                                    .MinimumShouldMatch(1)
                                ),
                                fq => nq.Bool(b2 => b2
                                    .Should(BuildRangeQueries(filters.Depth, "productAttributes.depth.value").ToArray())
                                    .MinimumShouldMatch(1)
                                )
                            )
                        )
                    )
                ))
                .Sort(s => Sorting<ProductSearchModel>(orderBy))
                .Aggregations(a => a
                    .Terms("category", t => t.Field("categoryName.keyword").Size(100))
                    .Nested("attributes", n => n
                        .Path("productAttributes")
                        .Aggregations(aa => aa
                            .Terms("color", tt => tt
                                .Field("productAttributes.primaryColor.value.name.keyword")
                                .Size(100)
                            )
                            .Terms("shape", tt => tt
                                .Field("productAttributes.shape.value.name.keyword")
                                .Size(100)
                            )
                            .Range("height", r => r.Field("productAttributes.height.value")
                                .Ranges(
                                    rr => rr.From(0).To(69),
                                    rr => rr.From(70).To(79),
                                    rr => rr.From(80).To(89),
                                    rr => rr.From(90).To(99),
                                    rr => rr.From(99)
                                )
                            )
                            .Range("width", r => r.Field("productAttributes.width.value")
                                .Ranges(
                                    rr => rr.From(0).To(99),
                                    rr => rr.From(100).To(149),
                                    rr => rr.From(150).To(199),
                                    rr => rr.From(200).To(249),
                                    rr => rr.From(250)
                                )
                            )
                            .Range("depth", r => r.Field("productAttributes.depth.value")
                                .Ranges(
                                    rr => rr.From(0).To(79),
                                    rr => rr.From(80).To(84),
                                    rr => rr.From(85).To(89),
                                    rr => rr.From(90).To(94),
                                    rr => rr.From(95)
                                )
                            )
                        )
                )));


            if (response.IsValid)
            {
                return new PagedResultsWithFilters<IEnumerable<ProductSearchModel>>(response.Total, itemsPerPage.Value)
                {
                    Data = response.Documents,
                    Filters = MapFilters(response)
                };
            }

            return default;
        }

        public async Task<PagedResultsWithFilters<IEnumerable<ProductSearchModel>>> GetPagedResultsWithFilters(
            string langauge, 
            IEnumerable<Guid> ids, 
            Guid? organisationId, 
            string orderBy, 
            QueryFilters filters,
            bool? isSeller)
        {
            var query = Query<ProductSearchModel>.Term(t => t.Language, langauge)
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

            var response = await _elasticClient.SearchAsync<ProductSearchModel>(q => q
                .TrackTotalHits()
                .From(ProductSearchConstants.Pagination.BeginningPage)
                .Size(ProductSearchConstants.Pagination.ProductsMaxSize)
                .Query(q => query)
                .PostFilter(fq => 
                    fq.Terms(t => t.Field("categoryName.keyword").Terms(filters.Category)) ||
                    fq.Nested(n => n
                        .Path("productAttributes")
                        .Query(nq => nq.Bool(b => b
                            .Filter(
                                fq => fq.Terms(t => t.Field("productAttributes.primaryColor.value.name.keyword").Terms(filters.Color)),
                                fq => fq.Terms(t => t.Field("productAttributes.shape.value.name.keyword").Terms(filters.Shape)),
                                fq => nq.Bool(b2 => b2
                                    .Should(BuildRangeQueries(filters.Height, "productAttributes.height.value").ToArray())
                                    .MinimumShouldMatch(1)
                                ),
                                fq => nq.Bool(b2 => b2
                                    .Should(BuildRangeQueries(filters.Width, "productAttributes.width.value").ToArray())
                                    .MinimumShouldMatch(1)
                                ),
                                fq => nq.Bool(b2 => b2
                                    .Should(BuildRangeQueries(filters.Depth, "productAttributes.depth.value").ToArray())
                                    .MinimumShouldMatch(1)
                                )
                            )
                        )
                    )
                ))
                .Sort(s => Sorting<ProductSearchModel>(orderBy))
                .Aggregations(a => a
                    .Terms("category", t => t.Field("categoryName.keyword").Size(100))
                    .Nested("attributes", n => n
                        .Path("productAttributes")
                        .Aggregations(aa => aa
                            .Terms("color", tt => tt
                                .Field("productAttributes.primaryColor.value.name.keyword")
                                .Size(100)
                            )
                            .Terms("shape", tt => tt
                                .Field("productAttributes.shape.value.name.keyword")
                                .Size(100)
                            )
                            .Range("height", r => r.Field("productAttributes.height.value")
                                .Ranges(
                                    rr => rr.From(0).To(69),
                                    rr => rr.From(70).To(79),
                                    rr => rr.From(80).To(89),
                                    rr => rr.From(90).To(99),
                                    rr => rr.From(99)
                                )
                            )
                            .Range("width", r => r.Field("productAttributes.width.value")
                                .Ranges(
                                    rr => rr.From(0).To(99),
                                    rr => rr.From(100).To(149),
                                    rr => rr.From(150).To(199),
                                    rr => rr.From(200).To(249),
                                    rr => rr.From(250)
                                )
                            )
                            .Range("depth", r => r.Field("productAttributes.depth.value")
                                .Ranges(
                                    rr => rr.From(0).To(79),
                                    rr => rr.From(80).To(84),
                                    rr => rr.From(85).To(89),
                                    rr => rr.From(90).To(94),
                                    rr => rr.From(95)
                                )
                            )
                        )
                )));


            if (response.IsValid)
            {
                return new PagedResultsWithFilters<IEnumerable<ProductSearchModel>>(response.Total, (int)response.Total)
                {
                    Data = response.Documents,
                    Filters = MapFilters(response)
                };
            }

            return default;
        }

        private Filter? MapBucketAggregate(string name, BucketAggregate bucketAggregate)
        {
            if (bucketAggregate.Items.FirstOrDefault() is KeyedBucket<object>)
            {
                var values = bucketAggregate.Items
                    .Cast<KeyedBucket<object>>()
                    .Select(b => b.Key?.ToString() ?? "")
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .ToList();

                return values.Count > 0
                    ? new Filter { Name = name, Values = values }
                    : null;
            }

            if (bucketAggregate.Items.FirstOrDefault() is RangeBucket)
            {
                var values = bucketAggregate.Items
                    .Cast<RangeBucket>()
                    .Select(rangeBucket =>
                    {
                        var from = rangeBucket.From ?? 0;
                        var to = rangeBucket.To ?? double.PositiveInfinity;

                        return double.IsPositiveInfinity(to)
                                ? $"{from:0}+"
                                : $"{from:0}-{(to - 1):0}"; ;
                    })
                    .ToList();

                return values.Count > 0
                    ? new Filter { Name = name, Values = values }
                    : null;
            }

            return null;
        }

        private IEnumerable<QueryContainer> BuildRangeQueries(IEnumerable<string>? ranges, string field)
        {
            var result = new List<QueryContainer>();

            if (ranges == null)
                return result;

            foreach (var text in ranges)
            {
                if (string.IsNullOrWhiteSpace(text)) continue;

                double gte;
                double? lt;

                if (text.EndsWith("+"))
                {
                    if (!double.TryParse(text.TrimEnd('+'), out gte)) continue;
                    lt = null;
                }
                else
                {
                    var parts = text.Split('-');

                    if (parts.Length != 2) continue;

                    if (!double.TryParse(parts[0], out gte)) continue;
                    if (!double.TryParse(parts[1], out var to)) continue;

                    lt = to + 1;
                }

                result.Add(new NumericRangeQuery
                {
                    Field = field,
                    GreaterThanOrEqualTo = gte,
                    LessThan = lt
                });
            }

            return result;
        }
    }
}