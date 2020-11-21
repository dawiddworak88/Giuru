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
using Catalog.Api.v1.Areas.Products.SearchModels;
using System;
using Foundation.Extensions.ExtensionMethods;

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

                foreach (var imageId in model.Images.OrEmptyIfNull())
                {
                    var productImage = new ProductImage
                    {
                        MediaId = imageId,
                        ProductId = product.Id
                    };

                    await this.catalogContext.ProductImages.AddAsync(this.entityService.EnrichEntity(productImage));
                }

                foreach (var videoId in model.Videos.OrEmptyIfNull())
                {
                    var productVideo = new ProductVideo
                    {
                        MediaId = videoId,
                        ProductId = product.Id
                    };

                    await this.catalogContext.ProductVideos.AddAsync(this.entityService.EnrichEntity(productVideo));
                }

                foreach (var fileId in model.Files.OrEmptyIfNull())
                {
                    var productFile = new ProductFile
                    {
                        MediaId = fileId,
                        ProductId = product.Id
                    };

                    await this.catalogContext.ProductFiles.AddAsync(this.entityService.EnrichEntity(productFile));
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

        public async Task<bool> IsEmptyAsync()
        {
            var count = await this.productSearchRepository.CountAllAsync();

            if (count.HasValue && count.Value == 0)
            {
                return true;
            }

            return false;
        }

        public async Task IndexAllAsync()
        {
            foreach (var productId in catalogContext.Products.Select(x => x.Id).ToList())
            {
                await this.productIndexingRepository.IndexAsync(productId);
            }
        }

        public async Task<ProductResultModel> UpdateAsync(CreateUpdateProductModel model)
        {
            return default;
        }

        public async Task DeleteAsync(DeleteProductModel model)
        {
            throw new NotSupportedException();
        }

        public async Task<PagedResults<IEnumerable<ProductResultModel>>> GetAsync(GetProductsModel model)
        {
            var searchResults = await this.productSearchRepository.GetAsync(model.Language, model.CategoryId, model.OrganisationId, model.SearchTerm, model.PageIndex, model.ItemsPerPage);
            return await this.MapToPageResultsAsync(searchResults, model.Language);
        }

        public async Task<PagedResults<IEnumerable<ProductResultModel>>> GetByIdsAsync(GetProductsByIdsModel model)
        {
            var searchResults = await this.productSearchRepository.GetAsync(model.Language, model.Ids);
            return await this.MapToPageResultsAsync(searchResults, model.Language);
        }

        public async Task<ProductResultModel> GetByIdAsync(GetProductModel model)
        {
            var searchResultItem = await this.productSearchRepository.GetAsync(model.Id.Value, model.Language);

            if (searchResultItem != null)
            {
                var productSearchModel = this.MapProductSearchModelToProductResult(searchResultItem);

                if (!searchResultItem.PrimaryProductIdHasValue)
                {
                    var productVariants = await this.productSearchRepository.GetProductVariantsAsync(searchResultItem.ProductId, model.Language);
                    productSearchModel.ProductVariants = productVariants?.Data?.Select(x => x.ProductId);
                }

                return productSearchModel;
            }

            return default;
        }

        public IEnumerable<string> GetProductSuggestions(GetProductSuggestionsModel model)
        {
            return this.productSearchRepository.GetProductSuggestions(model.SearchTerm, model.Size);
        }

        private async Task<PagedResults<IEnumerable<ProductResultModel>>> MapToPageResultsAsync(PagedResults<IEnumerable<ProductSearchModel>> searchResults, string language)
        {
            if (searchResults?.Data != null && searchResults.Data.Any())
            {
                var products = new List<ProductResultModel>();

                foreach (var searchResultItem in searchResults.Data)
                {
                    var productSearchModel = this.MapProductSearchModelToProductResult(searchResultItem);

                    if (!searchResultItem.PrimaryProductIdHasValue)
                    {
                        var productVariants = await this.productSearchRepository.GetProductVariantsAsync(searchResultItem.ProductId, language);
                        productSearchModel.ProductVariants = productVariants?.Data?.Select(x => x.ProductId);
                    }

                    products.Add(productSearchModel);
                }

                return new PagedResults<IEnumerable<ProductResultModel>>(searchResults.Total, searchResults.PageSize)
                {
                    Data = products
                };
            }

            return default;
        }

        private ProductResultModel MapProductSearchModelToProductResult(ProductSearchModel searchResultItem)
        { 
            return new ProductResultModel
            {
                Id = searchResultItem.ProductId,
                PrimaryProductId = searchResultItem.PrimaryProductId,
                Images = searchResultItem.Images,
                Files = searchResultItem.Files,
                Videos = searchResultItem.Videos,
                SellerId = searchResultItem.SellerId,
                BrandName = searchResultItem.BrandName,
                CategoryId = searchResultItem.CategoryId,
                CategoryName = searchResultItem.CategoryName,
                IsNew = searchResultItem.IsNew,
                IsProtected = searchResultItem.IsProtected,
                Sku = searchResultItem.Sku,
                Name = searchResultItem.Name,
                Description = searchResultItem.Description,
                FormData = searchResultItem.FormData,
                LastModifiedDate = searchResultItem.LastModifiedDate,
                CreatedDate = searchResultItem.CreatedDate
            };
        }
    }
}
