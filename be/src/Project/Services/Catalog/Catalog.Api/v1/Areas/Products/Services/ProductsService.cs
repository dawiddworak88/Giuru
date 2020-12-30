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
using Catalog.Api.v1.Areas.Products.SearchModels;
using Foundation.Extensions.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using Foundation.Extensions.Exceptions;
using System.Net;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Foundation.Localization.Services;
using System;
using Foundation.GenericRepository.Extensions;

namespace Catalog.Api.v1.Areas.Products.Services
{
    public class ProductsService : IProductsService
    {
        private readonly CatalogContext catalogContext;
        private readonly IProductSearchRepository productSearchRepository;
        private readonly IProductIndexingRepository productIndexingRepository;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly ICultureService cultureService;

        public ProductsService(
            CatalogContext catalogContext,
            IProductSearchRepository productSearchRepository,
            IProductIndexingRepository productIndexingRepository,
            IStringLocalizer<ProductResources> productLocalizer,
            ICultureService cultureService)
        {
            this.catalogContext = catalogContext;
            this.productSearchRepository = productSearchRepository;
            this.productIndexingRepository = productIndexingRepository;
            this.productLocalizer = productLocalizer;
            this.cultureService = cultureService;
        }

        public async Task<ProductResultModel> CreateAsync(CreateUpdateProductModel model)
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

            return await this.GetByIdAsync(new GetProductModel { Id = product.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
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

            return await this.GetByIdAsync(new GetProductModel { Id = product.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }

        public async Task DeleteAsync(DeleteProductModel model)
        {
            this.cultureService.SetCulture(model.Language);

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

        public async Task<PagedResults<IEnumerable<ProductResultModel>>> GetAsync(GetProductsModel model)
        {
            var searchResults = await this.productSearchRepository.GetAsync(model.Language, model.CategoryId, model.OrganisationId, model.IncludeProductVariants, model.SearchTerm, model.PageIndex, model.ItemsPerPage);
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

            return new PagedResults<IEnumerable<ProductResultModel>>(searchResults.Total, searchResults.PageSize)
            {
                Data = Enumerable.Empty<ProductResultModel>()
            };
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
