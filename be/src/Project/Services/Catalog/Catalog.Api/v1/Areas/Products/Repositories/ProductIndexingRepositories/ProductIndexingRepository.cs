using Catalog.Api.Configurations;
using Catalog.Api.Infrastructure;
using Catalog.Api.v1.Areas.Products.SearchModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Products.Repositories.ProductIndexingRepositories
{
    public class ProductIndexingRepository : IProductIndexingRepository
    {
        private readonly CatalogContext catalogContext;
        private readonly IElasticClient elasticClient;
        private readonly IOptions<AppSettings> configuration;

        public ProductIndexingRepository(
            CatalogContext catalogContext, 
            IElasticClient elasticClient,
            IOptions<AppSettings> configuration)
        {
            this.catalogContext = catalogContext;
            this.elasticClient = elasticClient;
            this.configuration = configuration;
        }

        public async Task IndexAsync(Guid productId)
        {
            var product = await this.catalogContext.Products.FirstOrDefaultAsync(x => x.Id == productId);

            if (product != null)
            {
                var existingProductVersions = await this.elasticClient.SearchAsync<ProductSearchModel>(q => q.Query(z => z.Term(p => p.ProductId, product.Id)));

                var descriptor = new BulkDescriptor();

                descriptor.DeleteMany<object>(existingProductVersions.Hits.Select(x => x.Id));

                foreach (var language in this.configuration.Value.SupportedCultures.Split(","))
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
                        dynamic document = new ExpandoObject();

                        document.language = language;
                        document.productId = product.Id;
                        document.categoryId = product.CategoryId;
                        document.categoryName = categoryTranslations.Name;
                        document.sellerId = product.Brand.SellerId;
                        document.brandName = product.Brand.Name;
                        document.isNew = product.IsNew;
                        document.isProtected = product.IsProtected;
                        document.images = this.catalogContext.ProductImages.Where(x => x.ProductId == product.Id && x.IsActive).Select(x => x.MediaId);
                        document.videos = this.catalogContext.ProductVideos.Where(x => x.ProductId == product.Id && x.IsActive).Select(x => x.MediaId);
                        document.files = this.catalogContext.ProductFiles.Where(x => x.ProductId == product.Id && x.IsActive).Select(x => x.MediaId);
                        document.isActive = product.IsActive;
                        document.sku = product.Sku;
                        document.formData = productTranslations.FormData;
                        document.name = productTranslations.Name;
                        document.primaryProductId = product.PrimaryProductId;
                        document.primaryProductIdHasValue = product.PrimaryProductId.HasValue;
                        document.description = productTranslations.Description;
                        document.lastModifiedDate = product.LastModifiedDate;
                        document.createdDate = product.CreatedDate;

                        descriptor.Index<object>(i => i
                            .Document((object)document));
                    }
                }

                await this.elasticClient.BulkAsync(descriptor);
            }
        }
    }
}
