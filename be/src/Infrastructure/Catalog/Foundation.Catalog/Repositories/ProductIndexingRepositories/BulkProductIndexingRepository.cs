using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.SearchModels;
using Foundation.Catalog.SearchModels.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Catalog.Repositories.ProductIndexingRepositories
{
    public class BulkProductIndexingRepository : IBulkProductIndexingRepository
    {
        private readonly CatalogContext _catalogContext;
        private readonly IElasticClient _elasticClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BulkProductIndexingRepository> _logger;
        private const int BATCH_SIZE = 300;
        private const int BULK_OPERATIONS_THRESHOLD = 5000;

        public BulkProductIndexingRepository(
            ILogger<BulkProductIndexingRepository> logger,
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
            var productIdsList = productIds.ToList();

            var categoryIds = await _catalogContext.Products
                .AsNoTracking()
                .Where(p => productIdsList.Contains(p.Id))
                .Select(p => p.CategoryId)
                .Distinct()
                .ToListAsync();

            var categorySchemas = await _catalogContext.CategorySchemas
                .AsNoTracking()
                .Where(x => categoryIds.Contains(x.CategoryId) && x.IsActive)
                .ToListAsync();

            var schemaCaches = new Dictionary<(Guid categoryId, string language), SchemaCache>();

            foreach (var schema in categorySchemas)
            {
                if (!string.IsNullOrWhiteSpace(schema.Schema))
                {
                    var key = (schema.CategoryId, schema.Language);
                    var parsed = JObject.Parse(schema.Schema);
                    schemaCaches.TryAdd(key, new SchemaCache(parsed, BuildEnumTitleCache(parsed)));
                }
            }

            await foreach (var batch in GetProductBatchesAsync(productIdsList))
            {
                await ProcessBatchAsync(batch, supportedCultures, schemaCaches);
            }

            await _elasticClient.Indices.RefreshAsync(Indices.Index<ProductSearchModel>());
        }

        private async IAsyncEnumerable<ProductBatchDto[]> GetProductBatchesAsync(List<Guid> productIds)
        {
            for (int i = 0; i < productIds.Count; i += BATCH_SIZE)
            {
                var batchIds = productIds.Skip(i).Take(BATCH_SIZE).ToList();

                var products = await _catalogContext.Products
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Where(p => batchIds.Contains(p.Id))
                    .Include(p => p.Translations)
                    .Include(p => p.Category).ThenInclude(c => c.Translations)
                    .Include(p => p.Brand)
                    .ToArrayAsync();

                var imagesList = await _catalogContext.ProductImages
                    .AsNoTracking()
                    .Where(i => batchIds.Contains(i.ProductId) && i.IsActive)
                    .Select(i => new { i.ProductId, i.MediaId })
                    .ToListAsync();

                var videosList = await _catalogContext.ProductVideos
                    .AsNoTracking()
                    .Where(v => batchIds.Contains(v.ProductId) && v.IsActive)
                    .Select(v => new { v.ProductId, v.MediaId })
                    .ToListAsync();

                var filesList = await _catalogContext.ProductFiles
                    .AsNoTracking()
                    .Where(f => batchIds.Contains(f.ProductId) && f.IsActive)
                    .Select(f => new { f.ProductId, f.MediaId })
                    .ToListAsync();

                var images = imagesList
                    .GroupBy(i => i.ProductId)
                    .ToDictionary(g => g.Key, g => g.Select(i => i.MediaId).ToArray());

                var videos = videosList
                    .GroupBy(v => v.ProductId)
                    .ToDictionary(g => g.Key, g => g.Select(v => v.MediaId).ToArray());

                var files = filesList
                    .GroupBy(f => f.ProductId)
                    .ToDictionary(g => g.Key, g => g.Select(f => f.MediaId).ToArray());

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
            Dictionary<(Guid categoryId, string language), SchemaCache> schemaCaches)
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

                        PopulateProductAttributes(document, productTranslations.FormData, product.CategoryId, language, schemaCaches);

                        bulk.Index<ProductSearchModel>(i => i
                            .Id(docId)
                            .Document(document));

                        operationCount++;

                        if (operationCount >= BULK_OPERATIONS_THRESHOLD)
                        {
                            await ExecuteBulkAsync(bulk, deleteIds);
                            bulk = new BulkDescriptor();
                            deleteIds = new List<string>();
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

        private void PopulateProductAttributes(
            ProductSearchModel document,
            string formData,
            Guid categoryId,
            string language,
            Dictionary<(Guid categoryId, string language), SchemaCache> schemaCaches)
        {
            if (string.IsNullOrWhiteSpace(formData))
                return;

            if (!schemaCaches.TryGetValue((categoryId, language), out var schemaCache))
            {
                schemaCache = schemaCaches
                    .Where(kv => kv.Key.categoryId == categoryId)
                    .Select(kv => kv.Value)
                    .FirstOrDefault();

                if (schemaCache is null)
                    return;
            }

            var formDataObject = JObject.Parse(formData);
            var schemaProperties = schemaCache.Schema["properties"] as JObject;
            if (schemaProperties is null)
                return;

            var attributes = new List<ProductAttributeSearchModel>();

            foreach (var schemaProperty in schemaProperties.Properties())
            {
                var formProperty = formDataObject.Property(schemaProperty.Name);
                if (formProperty is null)
                    continue;

                if (schemaProperty.Value is not JObject schemaPropertyObj)
                    continue;

                var attribute = BuildProductAttribute(formProperty, schemaPropertyObj, schemaCache.EnumTitles);
                if (attribute is not null)
                    attributes.Add(attribute);
            }

            document.ProductAttributes = attributes;
        }

        private ProductAttributeSearchModel BuildProductAttribute(JProperty formProperty, JObject schemaProperty, Dictionary<string, string> enumTitles)
        {
            var attribute = new ProductAttributeSearchModel
            {
                Key = formProperty.Name,
                Name = schemaProperty["title"]?.Value<string>()
            };

            var token = formProperty.Value;
            var schemaType = schemaProperty["type"]?.Value<string>();

            if (token.Type == JTokenType.Array)
            {
                var ids = new List<string>();
                var labels = new List<string>();

                foreach (var item in (JArray)token)
                {
                    var raw = item.ToString();
                    ids.Add(raw);
                    labels.Add(enumTitles.GetValueOrDefault(raw) ?? raw);
                }

                if (ids.Count == 0)
                    return null;

                attribute.ValueIds = ids.ToArray();
                attribute.ValueKeywords = labels.ToArray();
                return attribute;
            }

            switch (token.Type)
            {
                case JTokenType.Boolean:
                    attribute.ValueBoolean = token.Value<bool>();
                    attribute.ValueKeywords = [token.Value<bool>() ? "true" : "false"];
                    return attribute;

                case JTokenType.Integer:
                case JTokenType.Float:
                    attribute.ValueKeywords = [token.ToString()];
                    return attribute;

                case JTokenType.Object:
                    return null;

                default:
                    var s = Convert.ToString(token);
                    if (string.IsNullOrWhiteSpace(s))
                        return null;

                    if (schemaType == "boolean" && bool.TryParse(s, out var boolVal))
                    {
                        attribute.ValueBoolean = boolVal;
                        attribute.ValueKeywords = [boolVal ? "true" : "false"];
                        return attribute;
                    }

                    if (Guid.TryParse(s, out _))
                    {
                        attribute.ValueIds = [s];
                        attribute.ValueKeywords = [enumTitles.GetValueOrDefault(s) ?? s];
                        return attribute;
                    }

                    attribute.ValueText = s;
                    attribute.ValueKeywords = [s];

                    return attribute;
            }
        }

        private static Dictionary<string, string> BuildEnumTitleCache(JObject schema)
        {
            var cache = new Dictionary<string, string>(StringComparer.Ordinal);

            foreach (var entry in schema.SelectTokens("$.definitions..anyOf[*]"))
            {
                if (entry is not JObject entryObj)
                    continue;

                var enumVal = entryObj["enum"]?[0]?.ToString();
                var title = entryObj["title"]?.Value<string>();

                if (enumVal is not null && title is not null)
                    cache.TryAdd(enumVal, title);
            }

            return cache;
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

    internal sealed record SchemaCache(JObject Schema, Dictionary<string, string> EnumTitles);
}
