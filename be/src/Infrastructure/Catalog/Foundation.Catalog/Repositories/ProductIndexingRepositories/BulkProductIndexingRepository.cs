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
            var product = await _catalogContext.Products
                .Include(p => p.Translations)
                .Include(p => p.Category).ThenInclude(c => c.Translations)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(x => x.Id == productId);

            if (product != null)
            {
                await _elasticClient.DeleteByQueryAsync<ProductSearchModel>(q => q.Query(z => z.Term(p => p.ProductId, product.Id)));

                var descriptor = new BulkDescriptor();
                var supportedCultures = _configuration["SupportedCultures"].Split(",");

                foreach (var language in supportedCultures)
                {
                    var productTranslations = product.Translations.FirstOrDefault(x => x.Language == language && x.IsActive)
                        ?? product.Translations.FirstOrDefault(x => x.IsActive);
                    var categoryTranslations = product.Category.Translations.FirstOrDefault(x => x.Language == language && x.IsActive)
                        ?? product.Category.Translations.FirstOrDefault(x => x.IsActive);

                    if (productTranslations != null)
                    {
                        var document = CreateProductSearchModel(product, productTranslations, categoryTranslations, language);
                        await PopulateProductAttributesAsync(document, productTranslations.FormData, product.CategoryId, language);

                        descriptor.Index<ProductSearchModel>(i => i.Document(document));
                    }
                }

                await _elasticClient.DeleteByQueryAsync<ProductSearchModel>(q => q.Query(z => z.Term(p => p.ProductId, productId)));

                var response = await _elasticClient.BulkAsync(descriptor);

                if (!response.IsValid)
                {
                    _logger.LogError(response.DebugInformation);
                }
            }
        }

        private ProductSearchModel CreateProductSearchModel(Infrastructure.Products.Entities.Product product, Infrastructure.Products.Entities.ProductTranslation productTranslations, Infrastructure.Categories.Entites.CategoryTranslation categoryTranslations, string language)
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
                Images = _catalogContext.ProductImages.Where(x => x.ProductId == product.Id && x.IsActive).Select(x => x.MediaId),
                Videos = _catalogContext.ProductVideos.Where(x => x.ProductId == product.Id && x.IsActive).Select(x => x.MediaId),
                Files = _catalogContext.ProductFiles.Where(x => x.ProductId == product.Id && x.IsActive).Select(x => x.MediaId),
                IsActive = product.IsActive,
                Sku = product.Sku,
                FulfillmentTime = product.FulfillmentTime,
                FormData = productTranslations.FormData,
                Name = productTranslations.Name,
                NameSuggest = CreateCompletionField(productTranslations.Name, nameContexts),
                PrimaryProductId = product.PrimaryProductId,
                PrimaryProductSku = _catalogContext.Products.FirstOrDefault(x => x.Id == product.PrimaryProductId)?.Sku,
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

        private async Task PopulateProductAttributesAsync(ProductSearchModel document, string formData, Guid categoryId, string language)
        {
            if (!string.IsNullOrWhiteSpace(formData))
            {
                var categorySchema = await _catalogContext.CategorySchemas
                    .FirstOrDefaultAsync(x => x.CategoryId == categoryId && x.Language == language && x.IsActive)
                    ?? await _catalogContext.CategorySchemas
                    .FirstOrDefaultAsync(x => x.CategoryId == categoryId && x.IsActive);

                if (!string.IsNullOrWhiteSpace(categorySchema?.Schema))
                {
                    var formDataObject = JObject.Parse(formData);
                    var productAttributes = new Dictionary<string, object>();

                    foreach (var formDataProperty in formDataObject.Properties())
                    {
                        var propertyObject = (JObject)JObject.Parse(categorySchema.Schema)["properties"]?[formDataProperty.Name];

                        if (propertyObject != null)
                        {
                            var value = CreateAttributeValue(formDataProperty, propertyObject, categorySchema.Schema);
                            if (value != null)
                            {
                                productAttributes.Add(formDataProperty.Name, value);
                            }
                        }
                    }

                    document.ProductAttributes = productAttributes;
                }
            }
        }

        private object CreateAttributeValue(JProperty formDataProperty, JObject propertyObject, string schema)
        {
            if (formDataProperty.Value.Type != JTokenType.Array)
            {
                if (Guid.TryParse(formDataProperty.Value.ToString(), out var id))
                {
                    var title = (JValue)JObject.Parse(schema).SelectToken($"$.definitions...anyOf[?(@.enum[0] == '{id}')].title");
                    if (title != null)
                    {
                        return new
                        {
                            Name = propertyObject["title"]?.Value<string>(),
                            Value = new { Id = id, Name = title.Value<string>() }
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
                return CreateArrayAttributeValue((JArray)formDataProperty.Value, propertyObject, schema);
            }

            return null;
        }

        private object CreateSimpleAttributeValue(JProperty formDataProperty, JObject propertyObject)
        {
            var title = propertyObject["title"]?.Value<string>();
            return formDataProperty.Value.Type switch
            {
                JTokenType.Boolean => new { Name = title, Value = Convert.ToBoolean(formDataProperty.Value) },
                JTokenType.Float => new { Name = title, Value = (float)Convert.ToDouble(formDataProperty.Value) },
                JTokenType.Integer => new { Name = title, Value = Convert.ToInt32(formDataProperty.Value) },
                _ => new { Name = title, Value = Convert.ToString(formDataProperty.Value) }
            };
        }

        private object CreateArrayAttributeValue(JArray valueIdsArray, JObject propertyObject, string schema)
        {
            return new
            {
                Name = propertyObject["title"]?.Value<string>(),
                Value = valueIdsArray.Select(x => new
                {
                    Id = x,
                    Name = ((JValue)JObject.Parse(schema).SelectTokens($"$.definitions...anyOf[?(@.enum[0] == '{x}')].title").FirstOrDefault())?.Value<string>()
                })
            };
        }
    }
}
