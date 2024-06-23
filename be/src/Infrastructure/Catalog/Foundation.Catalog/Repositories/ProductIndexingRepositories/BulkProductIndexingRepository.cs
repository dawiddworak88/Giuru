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
        private readonly ILogger<BulkProductIndexingRepository> _logger;

        public BulkProductIndexingRepository(
            ILogger<BulkProductIndexingRepository> logger,
            CatalogContext catalogContext,
            IElasticClient elasticClient,
            IConfiguration configuration)
        {
            _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
            _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task IndexAsync(Guid productId)
        {
            var product = await _catalogContext.Products
                .Include(p => p.Translations)
                .Include(p => p.Category)
                .ThenInclude(c => c.Translations)
                .FirstOrDefaultAsync(x => x.Id == productId);

            if (product == null) return;

            await DeleteAsync(product.Id);

            var descriptor = new BulkDescriptor();

            foreach (var language in _configuration["SupportedCultures"].Split(","))
            {
                var document = CreateProductSearchModel(product, language);
                descriptor.Index<ProductSearchModel>(i => i.Document(document));
            }

            var response = await _elasticClient.BulkAsync(descriptor);

            if (!response.IsValid)
            {
                _logger.LogError($"Failed to index product {productId}: {response.DebugInformation}");
            }
        }

        private ProductSearchModel CreateProductSearchModel(Infrastructure.Products.Entities.Product product, string language)
        {
            var productTranslations = product.Translations.FirstOrDefault(t => t.Language == language && t.IsActive) ??
                                      product.Translations.FirstOrDefault(t => t.IsActive);
            var categoryTranslations = product.Category.Translations.FirstOrDefault(t => t.Language == language && t.IsActive) ??
                                       product.Category.Translations.FirstOrDefault(t => t.IsActive);

            if (productTranslations == null || categoryTranslations == null) return null;

            return new ProductSearchModel
            {
                Language = language,
                ProductId = product.Id,
                CategoryId = product.CategoryId,
                Ean = product.Ean,
                CategoryName = categoryTranslations.Name,
                CategoryNameSuggest = CreateCompletionField(categoryTranslations.Name, "isActive", product.Category.IsActive.ToString(), "language", language),
                SellerId = product.Brand.SellerId,
                BrandName = product.Brand.Name,
                BrandNameSuggest = CreateCompletionField(product.Brand.Name, "isActive", product.IsActive.ToString(), "primaryProductIdHasValue", product.PrimaryProductId.HasValue.ToString()),
                IsNew = product.IsNew,
                IsPublished = product.IsPublished,
                IsProtected = product.IsProtected,
                Images = _catalogContext.ProductImages.Where(x => x.ProductId == product.Id && x.IsActive).Select(x => x.MediaId),
                Videos = _catalogContext.ProductVideos.Where(x => x.ProductId == product.Id && x.IsActive).Select(x => x.MediaId),
                Files = _catalogContext.ProductFiles.Where(x => x.ProductId == product.Id && x.IsActive).Select(x => x.MediaId),
                IsActive = product.IsActive,
                Sku = product.Sku,
                FulfillmentTime = product.FulfillmentTime,
                FormData = productTranslations.FormData,
                Name = productTranslations.Name,
                NameSuggest = CreateCompletionField(productTranslations.Name, "isActive", product.IsActive.ToString(), "primaryProductIdHasValue", product.PrimaryProductId.HasValue.ToString()),
                PrimaryProductId = product.PrimaryProductId,
                PrimaryProductIdHasValue = product.PrimaryProductId.HasValue,
                Description = productTranslations.Description,
                LastModifiedDate = product.LastModifiedDate,
                CreatedDate = product.CreatedDate,
                ProductAttributes = ExtractProductAttributes(product, productTranslations)
            };
        }

        private Dictionary<string, object> ExtractProductAttributes(Infrastructure.Products.Entities.Product product, Infrastructure.Products.Entities.ProductTranslation translation)
        {
            var productAttributes = new Dictionary<string, object>();

            if (string.IsNullOrWhiteSpace(translation.FormData))
                return productAttributes;

            var formDataObject = JObject.Parse(translation.FormData);
            var categorySchema = _catalogContext.CategorySchemas
                                .FirstOrDefault(x => x.CategoryId == product.CategoryId && x.Language == translation.Language && x.IsActive) ??
                                _catalogContext.CategorySchemas
                                .FirstOrDefault(x => x.CategoryId == product.CategoryId && x.IsActive);

            if (categorySchema == null || string.IsNullOrWhiteSpace(categorySchema.Schema))
                return productAttributes;

            var schemaObject = JObject.Parse(categorySchema.Schema);

            foreach (var formDataProperty in formDataObject.Properties())
            {
                var key = formDataProperty.Name;
                var schemaProperty = (JObject)schemaObject["properties"]?[key];

                if (schemaProperty == null)
                    continue;

                var propertyTitle = schemaProperty["title"]?.ToString();
                var propertyType = schemaProperty["type"]?.ToString();

                if (formDataProperty.Value.Type == JTokenType.Array)
                {
                    var items = new List<object>();
                    foreach (var item in formDataProperty.Values())
                    {
                        items.Add(ResolveEnumTitle(item, schemaObject));
                    }
                    productAttributes.Add(key, new { Name = propertyTitle, Value = items });
                }
                else
                {
                    object value;
                    switch (propertyType)
                    {
                        case "integer":
                            value = formDataProperty.Value.Value<int>();
                            break;
                        case "number":
                            value = formDataProperty.Value.Value<float>();
                            break;
                        case "boolean":
                            value = formDataProperty.Value.Value<bool>();
                            break;
                        case "string":
                            if (schemaProperty["enum"] != null)
                            {
                                value = ResolveEnumTitle(formDataProperty.Value, schemaObject);
                            }
                            else
                            {
                                value = formDataProperty.Value.ToString();
                            }
                            break;
                        default:
                            value = formDataProperty.Value.ToString();
                            break;
                    }

                    productAttributes.Add(key, new { Name = propertyTitle, Value = value });
                }
            }

            return productAttributes;
        }

        private object ResolveEnumTitle(JToken enumValue, JObject schemaObject)
        {
            var titlePath = $"$.definitions...anyOf[?(@.enum[0] == '{enumValue}')].title";
            var title = (string)schemaObject.SelectToken(titlePath);
            return new { Id = enumValue, Name = title ?? enumValue.ToString() };
        }

        private async Task DeleteAsync(Guid sellerId)
        {
            var response = await _elasticClient.DeleteByQueryAsync<ProductSearchModel>(
                q => q.Query(z => z.Term(p => p.SellerId, sellerId)));

            if (!response.IsValid)
            {
                _logger.LogError($"Failed to delete products for sellerId {sellerId}: {response.DebugInformation}");
            }
        }

        private CompletionField CreateCompletionField(string input, string contextKey1, string contextValue1, string contextKey2, string contextValue2)
        {
            return new CompletionField
            {
                Input = input.Split(' '),
                Contexts = new Dictionary<string, IEnumerable<string>>
                {
                    {contextKey1, new[] {contextValue1}},
                    {contextKey2, new[] {contextValue2}}
                }
            };
        }
    }
}
