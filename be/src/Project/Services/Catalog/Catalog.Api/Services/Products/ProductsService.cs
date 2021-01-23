using Catalog.Api.ServicesModels.Products;
using System.Threading.Tasks;
using System.Linq;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using Catalog.Api.Repositories.Products.ProductSearchRepositories;
using Catalog.Api.Repositories.Products.ProductIndexingRepositories;
using Catalog.Api.SearchModels.Products;
using Foundation.Extensions.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using Foundation.Extensions.Exceptions;
using System.Net;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using System;
using Foundation.GenericRepository.Extensions;
using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.Infrastructure.Products.Entities;

namespace Catalog.Api.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly CatalogContext catalogContext;
        private readonly IProductSearchRepository productSearchRepository;
        private readonly IProductIndexingRepository productIndexingRepository;
        private readonly IStringLocalizer<ProductResources> productLocalizer;

        public ProductsService(
            CatalogContext catalogContext,
            IProductSearchRepository productSearchRepository,
            IProductIndexingRepository productIndexingRepository,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            this.catalogContext = catalogContext;
            this.productSearchRepository = productSearchRepository;
            this.productIndexingRepository = productIndexingRepository;
            this.productLocalizer = productLocalizer;
        }

        public async Task<ProductServiceModel> CreateAsync(CreateUpdateProductModel model)
        {
            var brand = catalogContext.Brands.FirstOrDefault(x => x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (brand == null)
            {
                throw new CustomException(this.productLocalizer.GetString("BrandNotFound"), (int)HttpStatusCode.NotFound);
            }

            var category = catalogContext.Categories.FirstOrDefault(x => x.Id == model.CategoryId && x.IsActive);

            if (category == null)
            {
                throw new CustomException(this.productLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NotFound);
            }

            var product = new Product
            {
                IsNew = model.IsNew,
                IsProtected = model.IsProtected,
                Sku = model.Sku,
                BrandId = brand.Id,
                CategoryId = category.Id,
                PrimaryProductId = model.PrimaryProductId
            };

            await this.catalogContext.Products.AddAsync(product.FillCommonProperties());

            var productTranslation = new ProductTranslation
            {
                Language = model.Language,
                Name = model.Name,
                Description = model.Description,
                FormData = model.FormData,
                ProductId = product.Id
            };

            await this.catalogContext.ProductTranslations.AddAsync(productTranslation.FillCommonProperties());

            foreach (var imageId in model.Images.OrEmptyIfNull())
            {
                var productImage = new ProductImage
                {
                    MediaId = imageId,
                    ProductId = product.Id
                };

                await this.catalogContext.ProductImages.AddAsync(productImage.FillCommonProperties());
            }

            foreach (var videoId in model.Videos.OrEmptyIfNull())
            {
                var productVideo = new ProductVideo
                {
                    MediaId = videoId,
                    ProductId = product.Id
                };

                await this.catalogContext.ProductVideos.AddAsync(productVideo.FillCommonProperties());
            }

            foreach (var fileId in model.Files.OrEmptyIfNull())
            {
                var productFile = new ProductFile
                {
                    MediaId = fileId,
                    ProductId = product.Id
                };

                await this.catalogContext.ProductFiles.AddAsync(productFile.FillCommonProperties());
            }

            await this.catalogContext.SaveChangesAsync();

            await this.productIndexingRepository.IndexAsync(product.Id);

            return await this.GetByIdAsync(new GetProductServiceModel { Id = product.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
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

        public async Task<ProductServiceModel> UpdateAsync(CreateUpdateProductModel model)
        {
            var brand = catalogContext.Brands.FirstOrDefault(x => x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (brand == null)
            {
                throw new CustomException(this.productLocalizer.GetString("BrandNotFound"), (int)HttpStatusCode.NotFound);
            }

            var category = catalogContext.Categories.FirstOrDefault(x => x.Id == model.CategoryId && x.IsActive);

            if (category == null)
            {
                throw new CustomException(this.productLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NotFound);
            }

            var product = await this.catalogContext.Products.FirstOrDefaultAsync(x => x.Id == model.Id && x.Brand.SellerId == model.OrganisationId && x.IsActive);

            if (product == null)
            {
                throw new CustomException(this.productLocalizer.GetString("ProductNotFound"), (int)HttpStatusCode.NotFound);
            }
            
            product.IsNew = model.IsNew;
            product.IsProtected = model.IsProtected;
            product.Sku = model.Sku;
            product.BrandId = brand.Id;
            product.CategoryId = category.Id;
            product.PrimaryProductId = model.PrimaryProductId;
            product.LastModifiedDate = DateTime.UtcNow;

            var productTranslation = await this.catalogContext.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == product.Id && x.Language == model.Language && x.IsActive);

            if (productTranslation != null)
            {
                productTranslation.Name = model.Name;
                productTranslation.Description = model.Description;
                productTranslation.FormData = model.FormData;
            }
            else
            {
                var newProductTranslation = new ProductTranslation
                {
                    ProductId = product.Id,
                    Name = model.Name,
                    Description = model.Description,
                    FormData = model.FormData
                };

                this.catalogContext.ProductTranslations.Add(newProductTranslation.FillCommonProperties());
            }

            var productImages = this.catalogContext.ProductImages.Where(x => x.ProductId == model.Id && x.IsActive);

            foreach (var productImage in productImages.OrEmptyIfNull())
            {
                this.catalogContext.ProductImages.Remove(productImage);
            }

            foreach (var imageId in model.Images.OrEmptyIfNull())
            {
                var productImage = new ProductImage
                {
                    MediaId = imageId,
                    ProductId = product.Id
                };

                await this.catalogContext.ProductImages.AddAsync(productImage.FillCommonProperties());
            }

            var productVideos = this.catalogContext.ProductVideos.Where(x => x.ProductId == model.Id && x.IsActive);

            foreach (var productVideo in productVideos.OrEmptyIfNull())
            {
                this.catalogContext.ProductVideos.Remove(productVideo);
            }

            foreach (var videoId in model.Videos.OrEmptyIfNull())
            {
                var productVideo = new ProductVideo
                {
                    MediaId = videoId,
                    ProductId = product.Id
                };

                await this.catalogContext.ProductVideos.AddAsync(productVideo.FillCommonProperties());
            }

            var productFiles = this.catalogContext.ProductFiles.Where(x => x.ProductId == model.Id && x.IsActive);

            foreach (var productFile in productFiles.OrEmptyIfNull())
            {
                this.catalogContext.ProductFiles.Remove(productFile);
            }

            foreach (var fileId in model.Files.OrEmptyIfNull())
            {
                var productFile = new ProductFile
                {
                    MediaId = fileId,
                    ProductId = product.Id
                };

                await this.catalogContext.ProductFiles.AddAsync(productFile.FillCommonProperties());
            }

            await this.catalogContext.SaveChangesAsync();

            await this.productIndexingRepository.IndexAsync(product.Id);

            return await this.GetByIdAsync(new GetProductServiceModel { Id = product.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }

        public async Task DeleteAsync(DeleteProductServiceModel model)
        {
            var product = await this.catalogContext.Products.FirstOrDefaultAsync(x => x.Id == model.Id && x.Brand.SellerId == model.OrganisationId && x.IsActive);

            if (product == null)
            {
                throw new CustomException(this.productLocalizer.GetString("ProductNotFound"), (int)HttpStatusCode.NotFound);
            }

            if (await this.catalogContext.Products.AnyAsync(x => x.PrimaryProductId == model.Id && x.Brand.SellerId == model.OrganisationId && x.IsActive))
            {
                throw new CustomException(this.productLocalizer.GetString("ProductVariantsDeleteProductConflict"), (int)HttpStatusCode.Conflict);
            }

            product.IsActive = false;

            await this.catalogContext.SaveChangesAsync();

            await this.productIndexingRepository.IndexAsync(product.Id);
        }

        public async Task<PagedResults<IEnumerable<ProductServiceModel>>> GetAsync(GetProductsServiceModel model)
        {
            var searchResults = await this.productSearchRepository.GetAsync(
                model.Language, 
                model.CategoryId, 
                model.OrganisationId, 
                model.HasPrimaryProduct,
                model.SearchTerm,
                model.PageIndex, 
                model.ItemsPerPage,
                model.OrderBy);

            return await this.MapToPageResultsAsync(searchResults, model.Language);
        }

        public async Task<PagedResults<IEnumerable<ProductServiceModel>>> GetByIdsAsync(GetProductsByIdsServiceModel model)
        {
            var searchResults = await this.productSearchRepository.GetAsync(model.Language, model.Ids, model.OrderBy);
            return await this.MapToPageResultsAsync(searchResults, model.Language);
        }

        public async Task<ProductServiceModel> GetByIdAsync(GetProductServiceModel model)
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

        public IEnumerable<string> GetProductSuggestions(GetProductSuggestionsServiceModel model)
        {
            return this.productSearchRepository.GetProductSuggestions(model.SearchTerm, model.Size, model.Language);
        }

        private async Task<PagedResults<IEnumerable<ProductServiceModel>>> MapToPageResultsAsync(PagedResults<IEnumerable<ProductSearchModel>> searchResults, string language)
        {
            if (searchResults?.Data != null && searchResults.Data.Any())
            {
                var products = new List<ProductServiceModel>();

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

                return new PagedResults<IEnumerable<ProductServiceModel>>(searchResults.Total, searchResults.PageSize)
                {
                    Data = products
                };
            }

            return new PagedResults<IEnumerable<ProductServiceModel>>(searchResults.Total, searchResults.PageSize)
            {
                Data = Enumerable.Empty<ProductServiceModel>()
            };
        }

        private ProductServiceModel MapProductSearchModelToProductResult(ProductSearchModel searchResultItem)
        { 
            return new ProductServiceModel
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
