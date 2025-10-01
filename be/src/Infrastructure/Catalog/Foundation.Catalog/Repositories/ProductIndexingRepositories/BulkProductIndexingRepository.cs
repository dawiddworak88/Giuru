using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.SearchModels.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Catalog.Repositories.ProductIndexingRepositories
{
    public class BulkProductIndexingRepository : IBulkProductIndexingRepository
    {
        private readonly CatalogContext _catalogContext;
        private readonly IElasticClient _elasticClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductIndexingRepository> _logger;
        private const int BATCH_SIZE = 300;
        private const int BULK_OPERATIONS_THRESHOLD = 5000;

        public BulkProductIndexingRepository(
            ILogger<ProductIndexingRepository> logger,
            CatalogContext catalogContext,
            IElasticClient elasticClient,
            IConfiguration configuration)
        {
            _catalogContext = catalogContext;
            _elasticClient = elasticClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task DeleteAsync(Guid sellerId)
        {
            await _elasticClient.DeleteByQueryAsync<ProductSearchModel>(q => q.Query(z => z.Term(p => p.SellerId, sellerId)));
        }

        public async Task IndexAsync(Guid productId)
        {
            await IndexBatchAsync(new[] { productId });
        }

        public async Task IndexBatchAsync(IEnumerable<Guid> productIds)
        {
            var supportedCultures = _configuration["SupportedCultures"].Split(",");
            var schemaCaches = new Dictionary<(Guid categoryId, string language), JObject>();

            await foreach (var batch in GetProductBatchesAsync(productIds))
            {
                await ProcessBatchAsync(batch, supportedCultures, schemaCaches);
            }

            await _elasticClient.Indices.RefreshAsync(Indices.Index<ProductSearchModel>());
        }

        private async IAsyncEnumerable<ProductBatchDto[]> GetProductBatchesAsync(IEnumerable<Guid> productIds)
        {
            var productIdsList = productIds.ToList();
            
            for (int i = 0; i < productIdsList.Count; i += BATCH_SIZE)
            {
                var batchIds = productIdsList.Skip(i).Take(BATCH_SIZE).ToList();
                
                var products = await _catalogContext.Products
                    .AsNoTracking()
                    .Where(p => batchIds.Contains(p.Id))
                    .Include(p => p.Translations)
                    .Include(p => p.Category).ThenInclude(c => c.Translations)
                    .Include(p => p.Brand)
                    .ToArrayAsync();

                var images = await _catalogContext.ProductImages
                    .AsNoTracking()
                    .Where(i => batchIds.Contains(i.ProductId) && i.IsActive)
                    .GroupBy(i => i.ProductId)
                    .ToDictionaryAsync(g => g.Key, g => g.Select(i => i.MediaId).ToArray());

                var videos = await _catalogContext.ProductVideos
                    .AsNoTracking()
                    .Where(v => batchIds.Contains(v.ProductId) && v.IsActive)
                    .GroupBy(v => v.ProductId)
                    .ToDictionaryAsync(g => g.Key, g => g.Select(v => v.MediaId).ToArray());

                var files = await _catalogContext.ProductFiles
                    .AsNoTracking()
                    .Where(f => batchIds.Contains(f.ProductId) && f.IsActive)
                    .GroupBy(f => f.ProductId)
                    .ToDictionaryAsync(g => g.Key, g => g.Select(f => f.MediaId).ToArray());

                var primaryProductIds = products.Where(p => p.PrimaryProductId.HasValue).Select(p => p.PrimaryProductId.Value).Distinct().ToList();
                var primaryProductSkus = primaryProductIds.Any() 
                    ? await _catalogContext.Products
                        .AsNoTracking()
                        .Where(p => primaryProductIds.Contains(p.Id))
                        .ToDictionaryAsync(p => p.Id, p => p.Sku)
                    : new Dictionary<Guid, string>();

                var batch = products.Select(p => new ProductBatchDto
                {
                    Product = p,
                    Images = images.GetValueOrDefault(p.Id, Array.Empty<Guid>()),
                    Videos = videos.GetValueOrDefault(p.Id, Array.Empty<Guid>()),
                    Files = files.GetValueOrDefault(p.Id, Array.Empty<Guid>()),
                    PrimaryProductSku = p.PrimaryProductId.HasValue && primaryProductSkus.TryGetValue(p.PrimaryProductId.Value, out var sku) ? sku : null
                }).ToArray();

                yield return batch;
            }
        }

        private async Task ProcessBatchAsync(
            ProductBatchDto[] batch, 
            string[] supportedCultures, 
            Dictionary<(Guid categoryId, string language), JObject> schemaCaches)
        {
            var bulk = new BulkDescriptor();
            var operationCount = 0;
            var deleteIds = new List<string>();

            foreach (var item in batch)
            {
                var product = item.Product;
                
                foreach (var language in supportedCultures)
                {
                    var docId = $"{product.Id}_{language}";
                    deleteIds.Add(docId);

                    var productTranslations = product.Translations.FirstOrDefault(x => x.Language == language && x.IsActive)
                        ?? product.Translations.FirstOrDefault(x => x.IsActive);

                    if (productTranslations != null)
                    {
                        var document = CreateProductSearchModel(product, productTranslations, language, item);
                        await PopulateProductAttributesAsync(document, productTranslations.FormData, product.CategoryId, language, schemaCaches);

                        bulk.Index<ProductSearchModel>(i => i
                            .Id(docId)
                            .Document(document));

                        operationCount++;

                        if (operationCount >= BULK_OPERATIONS_THRESHOLD)
                        {
                            await ExecuteBulkAsync(bulk, deleteIds);
                            bulk = new BulkDescriptor();
                            deleteIds.Clear();
                            operationCount = 0;
                        }
                    }
                }
            }

            if (operationCount > 0)
            {
                await ExecuteBulkAsync(bulk, deleteIds);
            }
        }

        private async Task ExecuteBulkAsync(BulkDescriptor bulk, List<string> deleteIds)
        {
            if (deleteIds.Any())
            {
                var productIds = deleteIds.Select(id => id.Split('_')[0]).Distinct().Select(Guid.Parse);
                await _elasticClient.DeleteByQueryAsync<ProductSearchModel>(d => d
                    .Query(q => q.Terms(t => t.Field(f => f.ProductId).Terms(productIds))));
            }

            var response = await _elasticClient.BulkAsync(bulk);

            if (!response.IsValid)
            {
                _logger.LogError("Bulk indexing failed: {DebugInfo}", response.DebugInformation);
            }
        }

        private ProductSearchModel CreateProductSearchModel(
            Infrastructure.Products.Entities.Product product, 
            Infrastructure.Products.Entities.ProductTranslation productTranslations, 
            string language,
            ProductBatchDto batchItem)
        {
            var categoryContexts = new Dictionary<string, IEnumerable<string>>
            {
                { "isActive", new[] { product.Category.IsActive.ToString() } },
                { "language", new[] { language } }
            };

            var brandContexts = new Dictionary<string, IEnumerable<string>>
            {
                { "isActive", new[] { product.IsActive.ToString() } },
                { "primaryProductIdHasValue", new[] { product.PrimaryProductId.HasValue.ToString() } }
            };

            var nameContexts = new Dictionary<string, IEnumerable<string>>
            {
                { "isActive", new[] { product.IsActive.ToString() } },
                { "primaryProductIdHasValue", new[] { product.PrimaryProductId.HasValue.ToString() } }
            };

            var categoryTranslations = product.Category.Translations.FirstOrDefault(x => x.Language == language && x.IsActive)
                ?? product.Category.Translations.FirstOrDefault(x => x.IsActive);

            return new ProductSearchModel
            {
                Language = language,
                ProductId = product.Id,
                CategoryId = product.CategoryId,
                Ean = product.Ean,
                CategoryName = categoryTranslations?.Name,
                CategoryNameSuggest = CreateCompletionField(categoryTranslations?.Name, categoryContexts),
                SellerId = product.Brand.SellerId,
                BrandName = product.Brand.Name,
                BrandNameSuggest = CreateCompletionField(product.Brand.Name, brandContexts),
                IsNew = product.IsNew,
                IsPublished = product.IsPublished,
                IsProtected = product.IsProtected,
                Images = batchItem.Images,
                Videos = batchItem.Videos,
                Files = batchItem.Files,
                IsActive = product.IsActive,
                Sku = product.Sku,
                FulfillmentTime = product.FulfillmentTime,
                FormData = productTranslations.FormData,
                Name = productTranslations.Name,
                NameSuggest = CreateCompletionField(productTranslations.Name, nameContexts),
                PrimaryProductId = product.PrimaryProductId,
                PrimaryProductSku = batchItem.PrimaryProductSku,
                PrimaryProductIdHasValue = product.PrimaryProductId.HasValue,
                Description = productTranslations.Description,
                LastModifiedDate = product.LastModifiedDate,
                CreatedDate = product.CreatedDate
            };
        }

        private CompletionField CreateCompletionField(string input, Dictionary<string, IEnumerable<string>> contexts)
        {
            return new CompletionField
            {
                Input = input?.Split(' ', StringSplitOptions.RemoveEmptyEntries),
                Contexts = contexts
            };
        }

        private async Task PopulateProductAttributesAsync(
            ProductSearchModel document, 
            string formData, 
            Guid categoryId, 
            string language,
            Dictionary<(Guid categoryId, string language), JObject> schemaCaches)
        {
            if (string.IsNullOrWhiteSpace(formData))
                return;

            var schemaKey = (categoryId, language);
            if (!schemaCaches.TryGetValue(schemaKey, out var schemaObject))
            {
                var categorySchema = await _catalogContext.CategorySchemas
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.CategoryId == categoryId && x.Language == language && x.IsActive)
                    ?? await _catalogContext.CategorySchemas
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.CategoryId == categoryId && x.IsActive);

                if (!string.IsNullOrWhiteSpace(categorySchema?.Schema))
                {
                    schemaObject = JObject.Parse(categorySchema.Schema);
                    schemaCaches[schemaKey] = schemaObject;
                }
                else
                {
                    return;
                }
            }

            var formDataObject = JObject.Parse(formData);
            var productAttributes = new Dictionary<string, object>();

            var formDataProperties = schemaObject["properties"]
                .Children<JProperty>()
                .Select(p => formDataObject.Property(p.Name))
                .Where(p => p != null);

            foreach (var formDataProperty in formDataProperties)
            {
                var propertyObject = (JObject)schemaObject["properties"]?[formDataProperty.Name];

                if (propertyObject != null)
                {
                    var value = CreateAttributeValue(formDataProperty, propertyObject, schemaObject);
                    if (value != null)
                    {
                        productAttributes.Add(formDataProperty.Name, value);
                    }
                }
            }

            document.ProductAttributes = productAttributes;
        }

        private object CreateAttributeValue(JProperty formDataProperty, JObject propertyObject, JObject schemaObject)
        {
            if (formDataProperty.Value.Type != JTokenType.Array)
            {
                if (Guid.TryParse(formDataProperty.Value.ToString(), out var id))
                {
                    var title = (JValue)schemaObject.SelectToken($"$.definitions...anyOf[?(@.enum[0] == '{id}')].title");
                    if (title != null)
                    {
                        return new AttributeValue
                        {
                            Name = propertyObject["title"]?.Value<string>(),
                            Value = new AttributeValueItem { Id = id.ToString(), Name = title.Value<string>() }
                        };
                    }
                }
                else
                {
                    return CreateSimpleAttributeValue(formDataProperty, propertyObject);
                }
            }
            else
            {
                return CreateArrayAttributeValue((JArray)formDataProperty.Value, propertyObject, schemaObject);
            }

            return null;
        }

        private object CreateSimpleAttributeValue(JProperty formDataProperty, JObject propertyObject)
        {
            var title = propertyObject["title"]?.Value<string>();
            return formDataProperty.Value.Type switch
            {
                JTokenType.Boolean => new AttributeValue { Name = title, Value = Convert.ToBoolean(formDataProperty.Value) },
                JTokenType.Float => new AttributeValue { Name = title, Value = (float)Convert.ToDouble(formDataProperty.Value) },
                JTokenType.Integer => new AttributeValue { Name = title, Value = Convert.ToInt32(formDataProperty.Value) },
                _ => new AttributeValue { Name = title, Value = Convert.ToString(formDataProperty.Value) }
            };
        }

        private object CreateArrayAttributeValue(JArray valueIdsArray, JObject propertyObject, JObject schemaObject)
        {
            return new AttributeValue
            {
                Name = propertyObject["title"]?.Value<string>(),
                Value = valueIdsArray.Select(x => new AttributeValueItem
                {
                    Id = x.ToString(),
                    Name = ((JValue)schemaObject.SelectTokens($"$.definitions...anyOf[?(@.enum[0] == '{x}')].title").FirstOrDefault())?.Value<string>()
                })
            };
        }
    }

    internal record ProductBatchDto
    {
        public Infrastructure.Products.Entities.Product Product { get; init; }
        public Guid[] Images { get; init; }
        public Guid[] Videos { get; init; }
        public Guid[] Files { get; init; }
        public string PrimaryProductSku { get; init; }
    }


    internal record AttributeValue
    {
        public string Name { get; init; }
        public object Value { get; init; }
    }

    internal record AttributeValueItem
    {
        public string Id { get; init; }
        public string Name { get; init; }
    }
}
