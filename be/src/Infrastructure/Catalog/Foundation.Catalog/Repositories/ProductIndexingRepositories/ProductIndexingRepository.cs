using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.SearchModels.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nest;
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

                    var categorySchemaTranslations = this.catalogContext.CategorySchemas.FirstOrDefault(x => x.CategoryId == product.Category.Id && x.Language == language && x.IsActive);

                    if (categorySchemaTranslations == null)
                    {
                        categorySchemaTranslations = this.catalogContext.CategorySchemas.FirstOrDefault(x => x.CategoryId == product.Category.Id && x.IsActive);
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
                            Schema = categorySchemaTranslations.Schema,
                            UiSchema = categorySchemaTranslations.UiSchema,
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

                        descriptor.Index<ProductSearchModel>(i => i
                            .Document(document));
                    }
                }

                await this.elasticClient.BulkAsync(descriptor);
            }
        }
    }
}
