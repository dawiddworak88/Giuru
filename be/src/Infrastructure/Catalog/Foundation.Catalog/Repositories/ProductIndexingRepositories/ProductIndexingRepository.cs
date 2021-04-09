using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.SearchModels;
using Foundation.Catalog.SearchModels.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        public ProductIndexingRepository(
            CatalogContext catalogContext, 
            IElasticClient elasticClient,
            IConfiguration configuration)
        {
            this.catalogContext = catalogContext;
            this.elasticClient = elasticClient;
            this.configuration = configuration;
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
                            IsProtected = product.IsProtected,
                            Images = this.catalogContext.ProductImages.Where(x => x.ProductId == product.Id && x.IsActive).Select(x => x.MediaId),
                            Videos = this.catalogContext.ProductVideos.Where(x => x.ProductId == product.Id && x.IsActive).Select(x => x.MediaId),
                            Files = this.catalogContext.ProductFiles.Where(x => x.ProductId == product.Id && x.IsActive).Select(x => x.MediaId),
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

                            if (!string.IsNullOrWhiteSpace(categorySchema.Schema))
                            {
                                var formDataObject = JObject.Parse(productTranslations.FormData);

                                var formDataProperties = formDataObject.Children();

                                var productAttributes = new List<ProductAttributeSearchModel>();

                                foreach (JProperty formDataProperty in formDataProperties)
                                {
                                    var categorySchemaObject = JObject.Parse(categorySchema.Schema);

                                    if (categorySchemaObject != null)
                                    {
                                        var propertyObject = (JObject)categorySchemaObject["properties"][formDataProperty.Name];

                                        var productAttributeSearchModel = new ProductAttributeSearchModel
                                        {
                                            Key = formDataProperty.Name
                                        };

                                        if (propertyObject != null)
                                        {
                                            productAttributeSearchModel.Name = propertyObject["title"].Value<string>();
                                        }

                                        if (formDataProperty.Value.Type != JTokenType.Array)
                                        {
                                            if (Guid.TryParse(formDataProperty.Value.ToString(), out var id))
                                            {
                                                JValue title = (JValue)categorySchemaObject.SelectToken($"$.definitions...anyOf[?(@.enum[0] == '{id}')].title");

                                                productAttributeSearchModel.Values = new List<ProductAttributeValueSearchModel>
                                                {
                                                    new ProductAttributeValueSearchModel
                                                    {
                                                        Id = id,
                                                        Value = title.Value<string>()
                                                    }
                                                };
                                            }
                                            else
                                            {
                                                productAttributeSearchModel.Values = new List<ProductAttributeValueSearchModel>
                                                {
                                                    new ProductAttributeValueSearchModel
                                                    {
                                                        Value = formDataProperty.Value.ToString()
                                                    }
                                                };
                                            }
                                        }
                                        else
                                        {
                                            var productAttributeValueSearchModels = new List<ProductAttributeValueSearchModel>();

                                            var valueIdsArray = (JArray)formDataProperty.Value;

                                            foreach (JValue valueId in valueIdsArray)
                                            {
                                                if (Guid.TryParse(valueId.ToString(), out var id))
                                                {
                                                    JValue title = (JValue)categorySchemaObject.SelectToken($"$.definitions...anyOf[?(@.enum[0] == '{id}')].title");

                                                    var productAttributeValueSearchModel = new ProductAttributeValueSearchModel
                                                    { 
                                                        Id = id,
                                                        Value = title.Value<string>()
                                                    };

                                                    productAttributeValueSearchModels.Add(productAttributeValueSearchModel);
                                                }
                                            }

                                            productAttributeSearchModel.Values = productAttributeValueSearchModels;
                                        }

                                        productAttributes.Add(productAttributeSearchModel);
                                    }
                                }

                                document.ProductAttributes = productAttributes;
                            }
                        }

                        descriptor.Index<ProductSearchModel>(i => i
                            .Document(document));
                    }
                }

                await this.elasticClient.BulkAsync(descriptor);
            }
        }
    }
}
