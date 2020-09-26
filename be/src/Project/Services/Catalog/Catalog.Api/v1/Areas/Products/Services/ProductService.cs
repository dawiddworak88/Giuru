using Catalog.Api.v1.Areas.Products.Models;
using System.Threading.Tasks;
using System.Linq;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using Catalog.Api.v1.Areas.Products.Repositories.ProductSearchRepositories;
using Catalog.Api.v1.Areas.Products.ResultModels;
using Catalog.Api.Infrastructure.Products.Entities;
using Catalog.Api.Infrastructure;
using Catalog.Api.v1.Areas.Products.Repositories.ProductIndexingRepositories;
using Foundation.GenericRepository.Services;

namespace Catalog.Api.v1.Areas.Products.Services
{
    public class ProductService : IProductService
    {
        private readonly CatalogContext catalogContext;
        private readonly IEntityService entityService;
        private readonly IProductSearchRepository productSearchRepository;
        private readonly IProductIndexingRepository productIndexingRepository;

        public ProductService(
            CatalogContext catalogContext,
            IEntityService entityService,
            IProductSearchRepository productSearchRepository,
            IProductIndexingRepository productIndexingRepository)
        {
            this.catalogContext = catalogContext;
            this.entityService = entityService;
            this.productSearchRepository = productSearchRepository;
            this.productIndexingRepository = productIndexingRepository;
        }

        public async Task<ProductResultModel> CreateAsync(CreateUpdateProductModel model)
        {
            var brand = catalogContext.Brands.FirstOrDefault(x => x.SellerId == model.OrganisationId.Value && x.IsActive);
            var category = catalogContext.Categories.FirstOrDefault(x => x.Id == model.CategoryId && x.IsActive);

            if (brand != null && category != null)
            {
                var product = new Product
                {
                    IsNew = model.IsNew,
                    IsProtected = model.IsProtected,
                    Sku = model.Sku,
                    BrandId = brand.Id,
                    CategoryId = category.Id,
                    PrimaryProductId = model.PrimaryProductId
                };

                await this.catalogContext.Products.AddAsync(this.entityService.EnrichEntity(product));

                var productTranslation = new ProductTranslation
                {
                    Language = model.Language,
                    Name = model.Name,
                    Description = model.Description,
                    FormData = model.FormData,
                    ProductId = product.Id
                };

                await this.catalogContext.ProductTranslations.AddAsync(this.entityService.EnrichEntity(productTranslation));

                if (model.Images != null && model.Images.Any())
                {
                    foreach (var imageId in model.Images)
                    {
                        var productImage = new ProductImage
                        {
                            MediaId = imageId,
                            ProductId = product.Id
                        };

                        await this.catalogContext.ProductImages.AddAsync(this.entityService.EnrichEntity(productImage));
                    }
                }

                if (model.Videos != null && model.Videos.Any())
                {
                    foreach (var videoId in model.Videos)
                    {
                        var productVideo = new ProductVideo
                        {
                            MediaId = videoId,
                            ProductId = product.Id
                        };

                        await this.catalogContext.ProductVideos.AddAsync(this.entityService.EnrichEntity(productVideo));
                    }
                }

                if (model.Files != null && model.Files.Any())
                {
                    foreach (var fileId in model.Files)
                    {
                        var productFile = new ProductFile
                        {
                            MediaId = fileId,
                            ProductId = product.Id
                        };

                        await this.catalogContext.ProductFiles.AddAsync(productFile);
                    }
                }

                await this.catalogContext.SaveChangesAsync();

                await this.productIndexingRepository.IndexAsync(product.Id);

                return new ProductResultModel
                {
                    Id = product.Id
                };
            }

            return default;
        }

        public async Task<ProductResultModel> UpdateAsync(CreateUpdateProductModel model)
        {
            return default;
        }

        public async Task DeleteAsync(DeleteProductModel model)
        {
        }

        public async Task<PagedResults<IEnumerable<ProductResultModel>>> GetAsync(GetProductsModel model)
        {
            var searchResults = await this.productSearchRepository.GetAsync(model.Language, model.CategoryId, model.OrganisationId, model.SearchTerm, model.PageIndex, model.ItemsPerPage);

            if (searchResults?.Data != null && searchResults.Data.Any())
            {
                var products = new List<ProductResultModel>();

                foreach (var searchResultItem in searchResults.Data)
                {
                    var product = new ProductResultModel
                    {
                        Id = searchResultItem.Id,
                        Sku = searchResultItem.Sku,
                        Name = searchResultItem.Name,
                        Description = searchResultItem.Description,
                        FormData = searchResultItem.FormData,
                        LastModifiedDate = searchResultItem.LastModifiedDate,
                        CreatedDate = searchResultItem.CreatedDate
                    };

                    products.Add(product);
                }

                return new PagedResults<IEnumerable<ProductResultModel>>(searchResults.Total, searchResults.PageSize)
                {
                    Data = products
                };
            }

            return default;
        }

        public async Task<ProductResultModel> GetByIdAsync(GetProductModel model)
        {
            return default;
        }
    }
}
