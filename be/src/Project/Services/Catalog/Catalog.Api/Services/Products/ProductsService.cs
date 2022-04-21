using Catalog.Api.ServicesModels.Products;
using System.Threading.Tasks;
using System.Linq;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
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
using Foundation.Catalog.Repositories.Products.ProductIndexingRepositories;
using Foundation.Catalog.SearchModels.Products;
using Foundation.EventBus.Abstractions;
using Catalog.Api.IntegrationEvents;
using Newtonsoft.Json.Linq;
using Foundation.Catalog.Repositories.ProductSearchRepositories;

namespace Catalog.Api.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly IEventBus eventBus;
        private readonly CatalogContext catalogContext;
        private readonly IProductSearchRepository productSearchRepository;
        private readonly IProductIndexingRepository productIndexingRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;

        public ProductsService(
            IEventBus eventBus,
            CatalogContext catalogContext,
            IProductSearchRepository productSearchRepository,
            IProductIndexingRepository productIndexingRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            this.eventBus = eventBus;
            this.catalogContext = catalogContext;
            this.productSearchRepository = productSearchRepository;
            this.productIndexingRepository = productIndexingRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
        }

        public async Task<Guid?> CreateAsync(CreateUpdateProductModel model)
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
                IsPublished = model.IsPublished,
                IsProtected = model.IsProtected,
                Sku = model.Sku,
                Ean = model.Ean,
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

            return product.Id;
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

        public async Task<Guid?> UpdateAsync(CreateUpdateProductModel model)
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
            product.IsPublished = model.IsPublished;
            product.IsProtected = model.IsProtected;
            product.Sku = model.Sku;
            product.Ean = model.Ean;
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
                    Language = model.Language,
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

            var message = new UpdatedProductIntegrationEvent
            {
                OrganisationId = model.OrganisationId,
                Language = model.Language,
                Username = model.Username,
                ProductId = model.Id,
                ProductName = model.Name,
                ProductSku = model.Sku
            };

            var updatedEan = new UpdatedEanProductIntegrationEvent
            {
                ProductId = model.Id,
                Ean = model.Ean
            };

            this.eventBus.Publish(message);
            this.eventBus.Publish(updatedEan);

            await this.catalogContext.SaveChangesAsync();

            await this.productIndexingRepository.IndexAsync(product.Id);
            
            return product.Id;
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
                model.IsNew,
                model.SearchTerm,
                model.PageIndex, 
                model.ItemsPerPage,
                model.OrderBy);

            return await this.MapToPageResultsAsync(searchResults, model.Language, model.OrganisationId);
        }

        public async Task<PagedResults<IEnumerable<ProductServiceModel>>> GetByIdsAsync(GetProductsByIdsServiceModel model)
        {
            var searchResults = await this.productSearchRepository.GetAsync(model.Language, model.OrganisationId, model.Ids, model.OrderBy);
            return await this.MapToPageResultsAsync(searchResults, model.Language, model.OrganisationId);
        }

        public async Task<ProductServiceModel> GetByIdAsync(GetProductByIdServiceModel model)
        {
            var searchResultItem = await this.productSearchRepository.GetByIdAsync(model.Id.Value, model.Language, model.OrganisationId);

            if (searchResultItem != null)
            {
                var productSearchModel = this.MapProductSearchModelToProductResult(searchResultItem);

                if (!searchResultItem.PrimaryProductIdHasValue)
                {
                    var productVariants = await this.productSearchRepository.GetProductVariantsAsync(searchResultItem.ProductId, model.Language, model.OrganisationId);
                    productSearchModel.ProductVariants = productVariants?.Data?.Select(x => x.ProductId);
                }

                return productSearchModel;
            }

            return default;
        }

        public async Task<ProductServiceModel> GetBySkuAsync(GetProductBySkuServiceModel model)
        {
            var searchResultItem = await this.productSearchRepository.GetBySkuAsync(model.Sku, model.Language, model.OrganisationId);

            if (searchResultItem != null)
            {
                var productSearchModel = this.MapProductSearchModelToProductResult(searchResultItem);

                if (!searchResultItem.PrimaryProductIdHasValue)
                {
                    var productVariants = await this.productSearchRepository.GetProductVariantsAsync(searchResultItem.ProductId, model.Language, model.OrganisationId);
                    productSearchModel.ProductVariants = productVariants?.Data?.Select(x => x.ProductId);
                }

                return productSearchModel;
            }

            return default;
        }

        public IEnumerable<string> GetProductSuggestions(GetProductSuggestionsServiceModel model)
        {
            return this.productSearchRepository.GetProductSuggestions(model.SearchTerm, model.Size, model.Language, model.OrganisationId);
        }

        private async Task<PagedResults<IEnumerable<ProductServiceModel>>> MapToPageResultsAsync(PagedResults<IEnumerable<ProductSearchModel>> searchResults, string language, Guid? organisationId)
        {
            if (searchResults?.Data != null && searchResults.Data.Any())
            {
                var products = new List<ProductServiceModel>();

                foreach (var searchResultItem in searchResults.Data)
                {
                    var productSearchModel = this.MapProductSearchModelToProductResult(searchResultItem);

                    if (!searchResultItem.PrimaryProductIdHasValue)
                    {
                        var productVariants = await this.productSearchRepository.GetProductVariantsAsync(searchResultItem.ProductId, language, organisationId);
                        productSearchModel.ProductVariants = productVariants?.Data?.Select(x => x.ProductId);
                    }

                    products.Add(productSearchModel);
                }

                return new PagedResults<IEnumerable<ProductServiceModel>>(searchResults.Total, searchResults.PageSize)
                {
                    Data = products
                };
            }

            return new PagedResults<IEnumerable<ProductServiceModel>>(PaginationConstants.EmptyTotal, PaginationConstants.DefaultPageSize)
            {
                Data = Enumerable.Empty<ProductServiceModel>()
            };
        }

        private ProductServiceModel MapProductSearchModelToProductResult(ProductSearchModel searchResultItem)
        {
            var productAttributes = new List<ProductAttributeServiceModel>();

            foreach (var productAttributeSearchModel in searchResultItem.ProductAttributes.OrEmptyIfNull())
            {
                var productAttributeObject = JObject.FromObject(productAttributeSearchModel.Value);

                if (productAttributeObject != null)
                {
                    var productAttribute = new ProductAttributeServiceModel
                    { 
                        Key = productAttributeSearchModel.Key,
                        Name = productAttributeObject["name"].Value<string>()
                    };

                    if (productAttributeObject["value"].Type == JTokenType.Array)
                    {
                        var valuesArray = (JArray)productAttributeObject["value"];

                        if (valuesArray != null)
                        {
                            productAttribute.Values = valuesArray.Children().Select(x => ((JObject)x)["name"].Value<string>());
                        }
                    }
                    else if (productAttributeObject["value"].Type == JTokenType.Object)
                    {
                        var valueObject = (JObject)productAttributeObject["value"];

                        if (valueObject != null)
                        {
                            productAttribute.Values = new string[] { valueObject["name"].Value<string>() };
                        }
                    }
                    else if (productAttributeObject["value"].Type == JTokenType.Boolean)
                    {
                        if (productAttributeObject["value"].Value<bool>())
                        {
                            productAttribute.Values = new string[] { this.globalLocalizer.GetString("Yes") };
                        }
                        else
                        {
                            productAttribute.Values = new string[] { this.globalLocalizer.GetString("No") };
                        }
                    }
                    else
                    {
                        productAttribute.Values = new string[] { productAttributeObject["value"].Value<string>() };
                    }

                    productAttributes.Add(productAttribute);
                }
            }

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
                Ean = searchResultItem.Ean,
                IsPublished = searchResultItem.IsPublished,
                IsProtected = searchResultItem.IsProtected,
                Sku = searchResultItem.Sku,
                Name = searchResultItem.Name,
                Description = searchResultItem.Description,
                FormData = searchResultItem.FormData,
                ProductAttributes = productAttributes,
                LastModifiedDate = searchResultItem.LastModifiedDate,
                CreatedDate = searchResultItem.CreatedDate
            };
        }

        public async Task TriggerCatalogIndexRebuildAsync(RebuildCatalogIndexServiceModel model)
        {
            var message = new RebuildCatalogSearchIndexIntegrationEvent
            {
                OrganisationId = model.OrganisationId,
                Language = model.Language,
                Username = model.Username
            };

            this.eventBus.Publish(message);
        }
    }
}
