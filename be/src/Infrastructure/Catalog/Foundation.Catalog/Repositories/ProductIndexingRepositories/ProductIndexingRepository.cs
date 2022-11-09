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

namespace Foundation.Catalog.Repositories.Products.ProductIndexingRepositories
{
    public class ProductIndexingRepository : IProductIndexingRepository
    {
        private readonly CatalogContext catalogContext;
        private readonly IElasticClient elasticClient;
        private readonly IConfiguration configuration;
        private readonly ILogger<ProductIndexingRepository> logger;

        public ProductIndexingRepository(
            ILogger<ProductIndexingRepository> logger,
            CatalogContext catalogContext, 
            IElasticClient elasticClient,
            IConfiguration configuration)
        {
            this.catalogContext = catalogContext;
            this.elasticClient = elasticClient;
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task DeleteAsync(Guid sellerId)
        {
            await this.elasticClient.DeleteByQueryAsync<ProductSearchModel>(q => q.Query(z => z.Term(p => p.SellerId, sellerId)));
        }

        public async Task IndexAsync(Guid productId)
        {
            var product = await this.catalogContext.Products.FirstOrDefaultAsync(x => x.Id == productId);

            if (product != null)
            {
                await this.elasticClient.DeleteByQueryAsync<ProductSearchModel>(q => q.Query(z => z.Term(p => p.ProductId, product.Id)));

                var descriptor = new BulkDescriptor();

                foreach (var language in this.configuration["SupportedCultures"].Split(","))
                {
                    var productTranslations = product.Translations.FirstOrDefault(x => x.Language == language && x.IsActive);

                    if (productTranslations == null)
                    {
                        productTranslations = product.Translations.FirstOrDefault(x => x.IsActive);
                    }

                    var categoryTranslations = product.Category.Translations.FirstOrDefault(x => x.Language == language && x.IsActive);

                    if (categoryTranslations == null)
                    {
                        categoryTranslations = product.Category.Translations.FirstOrDefault(x => x.IsActive);
                    }

                    if (productTranslations != null)
                    {
                        var document = new ProductSearchModel
                        { 
                            Language = language,
                            ProductId = product.Id,
                            CategoryId = product.CategoryId,
                            Ean = product.Ean,
                            CategoryName = categoryTranslations.Name,
                            CategoryNameSuggest = new CompletionField
                            {
                                Input = categoryTranslations.Name.Split(' '),
                                Contexts = new Dictionary<string, IEnumerable<string>>
                                {
                                    {
                                        "isActive",
                                        new[] { product.Category.IsActive.ToString() }
                                    },
                                    {
                                        "language",
                                        new[] { language.ToString() }
                                    }
                                }
                            },
                            SellerId = product.Brand.SellerId,
                            BrandName = product.Brand.Name,
                            BrandNameSuggest = new CompletionField
                            {
                                Input = new[] { product.Brand.Name },
                                Contexts = new Dictionary<string, IEnumerable<string>>
                                {
                                    {
                                        "isActive",
                                        new[] { product.IsActive.ToString() }
                                    },
                                    {
                                        "primaryProductIdHasValue",
                                        new[] { product.PrimaryProductId.HasValue.ToString() }
                                    }
                                }
                            },
                            IsNew = product.IsNew,
                            IsPublished = product.IsPublished,
                            IsProtected = product.IsProtected,
                            Images = this.catalogContext.ProductImages.Where(x => x.ProductId == product.Id && x.IsActive).Select(x => x.MediaId),
                            Videos = this.catalogContext.ProductVideos.Where(x => x.ProductId == product.Id && x.IsActive).Select(x => x.MediaId),
                            Files = this.catalogContext.ProductFiles.Where(x => x.ProductId == product.Id && x.IsActive).Select(x => x.MediaId),
                            Groups = this.catalogContext.ProductsGroups.Where(x => x.ProductId == product.Id && x.IsActive).Select(x => x.GroupId),
                            IsActive = product.IsActive,
                            Sku = product.Sku,
                            FormData = productTranslations.FormData,
                            Name = productTranslations.Name,
                            NameSuggest = new CompletionField
                            {
                                Input = new[] { productTranslations.Name },
                                Contexts = new Dictionary<string, IEnumerable<string>>
                                {
                                    {
                                        "isActive",
                                        new[] { product.IsActive.ToString() }
                                    },
                                    {
                                        "primaryProductIdHasValue",
                                        new[] { product.PrimaryProductId.HasValue.ToString() }
                                    }
                                }
                            },
                            PrimaryProductId = product.PrimaryProductId,
                            PrimaryProductIdHasValue = product.PrimaryProductId.HasValue,
                            Description = productTranslations.Description,
                            LastModifiedDate = product.LastModifiedDate,
                            CreatedDate = product.CreatedDate
                        };

                        if (!string.IsNullOrWhiteSpace(productTranslations.FormData))
                        {
                            var categorySchema = this.catalogContext.CategorySchemas.FirstOrDefault(x => x.CategoryId == product.CategoryId && x.Language == language && x.IsActive);

                            if (categorySchema == null)
                            {
                                categorySchema = this.catalogContext.CategorySchemas.FirstOrDefault(x => x.CategoryId == product.CategoryId && x.IsActive);
                            }

                            if (!string.IsNullOrWhiteSpace(categorySchema?.Schema))
                            {
                                var formDataObject = JObject.Parse(productTranslations.FormData);

                                var formDataProperties = formDataObject.Children();

                                var productAttributes = new Dictionary<string, object>();

                                foreach (JProperty formDataProperty in formDataProperties)
                                {
                                    var categorySchemaObject = JObject.Parse(categorySchema.Schema);

                                    if (categorySchemaObject != null)
                                    {
                                        string key = formDataProperty.Name;

                                        var propertyObject = (JObject)categorySchemaObject["properties"][formDataProperty.Name];

                                        if (propertyObject != null)
                                        {
                                            if (formDataProperty.Value.Type != JTokenType.Array)
                                            {
                                                if (Guid.TryParse(formDataProperty.Value.ToString(), out var id))
                                                {
                                                    JValue title = (JValue)categorySchemaObject.SelectToken($"$.definitions...anyOf[?(@.enum[0] == '{id}')].title");

                                                    var value = new
                                                    {
                                                        Name = propertyObject["title"].Value<string>(),
                                                        Value = new
                                                        {
                                                            Id = id,
                                                            Name = title.Value<string>()
                                                        }
                                                    };

                                                    productAttributes.Add(key, value);
                                                }
                                                else
                                                {
                                                    if (formDataProperty.Value.Type == JTokenType.Boolean)
                                                    {
                                                        var value = new
                                                        {
                                                            Name = propertyObject["title"].Value<string>(),
                                                            Value = Convert.ToBoolean(formDataProperty.Value)
                                                        };

                                                        productAttributes.Add(key, value);
                                                    }
                                                    else if (formDataProperty.Value.Type == JTokenType.Float)
                                                    {
                                                        var value = new
                                                        {
                                                            Name = propertyObject["title"].Value<string>(),
                                                            Value = (float)Convert.ToDouble(formDataProperty.Value)
                                                        };

                                                        productAttributes.Add(key, value);
                                                    }
                                                    else if (formDataProperty.Value.Type == JTokenType.Integer)
                                                    {
                                                        var value = new
                                                        {
                                                            Name = propertyObject["title"].Value<string>(),
                                                            Value = Convert.ToInt32(formDataProperty.Value)
                                                        };

                                                        productAttributes.Add(key, value);
                                                    }
                                                    else
                                                    {
                                                        var value = new
                                                        {
                                                            Name = propertyObject["title"].Value<string>(),
                                                            Value = Convert.ToString(formDataProperty.Value)
                                                        };

                                                        productAttributes.Add(key, value);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var valueIdsArray = (JArray)formDataProperty.Value;

                                                var value = new
                                                {
                                                    Name = propertyObject["title"].Value<string>(),
                                                    Value = valueIdsArray.Select(x => new
                                                    {
                                                        Id = x,
                                                        Name = ((JValue)categorySchemaObject.SelectTokens($"$.definitions...anyOf[?(@.enum[0] == '{x}')].title").FirstOrDefault())?.Value<string>()
                                                    })
                                                };

                                                productAttributes.Add(key, value);
                                            }
                                        }
                                    }
                                }

                                document.ProductAttributes = productAttributes;
                            }
                        }

                        descriptor.Index<ProductSearchModel>(i => i
                            .Document(document));
                    }
                }

                var response = await this.elasticClient.BulkAsync(descriptor);

                if (response.IsValid is false)
                {
                    this.logger.LogError(response.DebugInformation);
                }
            }
        }
    }
}
