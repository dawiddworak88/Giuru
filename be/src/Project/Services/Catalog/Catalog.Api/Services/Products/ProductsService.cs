using Catalog.Api.ServicesModels.Products;
using System.Threading.Tasks;
using System.Linq;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using Foundation.Extensions.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using System;
using Foundation.GenericRepository.Extensions;
using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.Infrastructure.Products.Entities;
using Foundation.Catalog.SearchModels.Products;
using Foundation.EventBus.Abstractions;
using Catalog.Api.IntegrationEvents;
using Newtonsoft.Json.Linq;
using Foundation.Catalog.Repositories.ProductSearchRepositories;
using System.Diagnostics;
using Foundation.GenericRepository.Definitions;
using Foundation.Catalog.Repositories.ProductIndexingRepositories;
using Foundation.Search.Paginations;

namespace Catalog.Api.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly IEventBus _eventBus;
        private readonly CatalogContext _context;
        private readonly IProductSearchRepository _productSearchRepository;
        private readonly IProductIndexingRepository _productIndexingRepository;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ProductResources> _productLocalizer;

        public ProductsService(
            IEventBus eventBus,
            CatalogContext catalogContext,
            IProductSearchRepository productSearchRepository,
            IProductIndexingRepository productIndexingRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            _eventBus = eventBus;
            _context = catalogContext;
            _productSearchRepository = productSearchRepository;
            _productIndexingRepository = productIndexingRepository;
            _globalLocalizer = globalLocalizer;
            _productLocalizer = productLocalizer;
        }

        public async Task<Guid?> CreateAsync(CreateUpdateProductModel model)
        {
            var brand = _context.Brands.FirstOrDefault(x => x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (brand is null)
            {
                throw new NotFoundException(_productLocalizer.GetString("BrandNotFound"));
            }

            var category = _context.Categories.FirstOrDefault(x => x.Id == model.CategoryId && x.IsActive);

            if (category is null)
            {
                throw new NotFoundException(_productLocalizer.GetString("CategoryNotFound"));
            }

            if (_context.Products.Any(x => x.Sku == model.Sku && x.IsActive))
            {
                throw new ConflictException(_productLocalizer.GetString("ProductSkuConflict"));
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
                PrimaryProductId = model.PrimaryProductId,
                FulfillmentTime = model.FulfillmentTime,
            };

            await _context.Products.AddAsync(product.FillCommonProperties());

            var productTranslation = new ProductTranslation
            {
                Language = model.Language,
                Name = model.Name,
                Description = model.Description,
                FormData = model.FormData,
                ProductId = product.Id
            };

            await _context.ProductTranslations.AddAsync(productTranslation.FillCommonProperties());

            foreach (var imageId in model.Images.OrEmptyIfNull())
            {
                var productImage = new ProductImage
                {
                    MediaId = imageId,
                    ProductId = product.Id
                };

                await _context.ProductImages.AddAsync(productImage.FillCommonProperties());
            }

            foreach (var videoId in model.Videos.OrEmptyIfNull())
            {
                var productVideo = new ProductVideo
                {
                    MediaId = videoId, 
                    ProductId = product.Id
                };

                await _context.ProductVideos.AddAsync(productVideo.FillCommonProperties());
            }

            foreach (var fileId in model.Files.OrEmptyIfNull())
            {
                var productFile = new ProductFile
                {
                    MediaId = fileId,
                    ProductId = product.Id
                };

                await _context.ProductFiles.AddAsync(productFile.FillCommonProperties());
            }

            await _context.SaveChangesAsync();

            await _productIndexingRepository.IndexAsync(product.Id);

            return product.Id;
        }

        public async Task<bool> IsEmptyAsync()
        {
            var count = await _productSearchRepository.CountAllAsync();

            if (count.HasValue && count.Value == 0)
            {
                return true;
            }

            return false;
        }

        public async Task<Guid?> UpdateAsync(CreateUpdateProductModel model)
        {
            using var source = new ActivitySource(this.GetType().Name);

            var brand = _context.Brands.FirstOrDefault(x => x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (brand is null)
            {
                throw new NotFoundException(_productLocalizer.GetString("BrandNotFound"));
            }

            var category = _context.Categories.FirstOrDefault(x => x.Id == model.CategoryId && x.IsActive);

            if (category is null)
            {
                throw new NotFoundException(_productLocalizer.GetString("CategoryNotFound"));
            }

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == model.Id && x.Brand.SellerId == model.OrganisationId && x.IsActive);

            if (product is null)
            {
                throw new NotFoundException(_productLocalizer.GetString("ProductNotFound"));
            }

            if (_context.Products.Any(x => x.Sku == model.Sku && x.Id != model.Id && x.IsActive))
            {
                throw new ConflictException(_productLocalizer.GetString("ProductSkuConflict"));
            }

            product.IsNew = model.IsNew;
            product.IsPublished = model.IsPublished;
            product.IsProtected = model.IsProtected;
            product.Sku = model.Sku;
            product.Ean = model.Ean;
            product.BrandId = brand.Id;
            product.CategoryId = category.Id;
            product.FulfillmentTime = model.FulfillmentTime;
            product.PrimaryProductId = model.PrimaryProductId;
            product.LastModifiedDate = DateTime.UtcNow;

            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == product.Id && x.Language == model.Language && x.IsActive);

            if (productTranslation is not null)
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

                _context.ProductTranslations.Add(newProductTranslation.FillCommonProperties());
            }

            var productImages = _context.ProductImages.Where(x => x.ProductId == model.Id && x.IsActive);

            foreach (var productImage in productImages.OrEmptyIfNull())
            {
                _context.ProductImages.Remove(productImage);
            }

            foreach (var imageId in model.Images.OrEmptyIfNull())
            {
                var productImage = new ProductImage
                {
                    MediaId = imageId,
                    ProductId = product.Id
                };

                await _context.ProductImages.AddAsync(productImage.FillCommonProperties());
            }

            var productVideos = _context.ProductVideos.Where(x => x.ProductId == model.Id && x.IsActive);

            foreach (var productVideo in productVideos.OrEmptyIfNull())
            {
                _context.ProductVideos.Remove(productVideo);
            }

            foreach (var videoId in model.Videos.OrEmptyIfNull())
            {
                var productVideo = new ProductVideo
                {
                    MediaId = videoId,
                    ProductId = product.Id
                };

                await _context.ProductVideos.AddAsync(productVideo.FillCommonProperties());
            }

            var productFiles = _context.ProductFiles.Where(x => x.ProductId == model.Id && x.IsActive);

            foreach (var productFile in productFiles.OrEmptyIfNull())
            {
                _context.ProductFiles.Remove(productFile);
            }

            foreach (var fileId in model.Files.OrEmptyIfNull())
            {
                var productFile = new ProductFile
                {
                    MediaId = fileId,
                    ProductId = product.Id
                };

                await _context.ProductFiles.AddAsync(productFile.FillCommonProperties());
            }

            var message = new UpdatedProductIntegrationEvent
            {
                OrganisationId = model.OrganisationId,
                Language = model.Language,
                Username = model.Username,
                ProductId = model.Id,
                ProductName = model.Name,
                ProductSku = model.Sku,
                ProductEan = model.Ean
            };

            using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {message.GetType().Name}");
            
            _eventBus.Publish(message);

            await _context.SaveChangesAsync();

            await _productIndexingRepository.IndexAsync(product.Id);
            
            return product.Id;
        }

        public async Task DeleteAsync(DeleteProductServiceModel model)
        {
            using var source = new ActivitySource(this.GetType().Name);

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == model.Id && x.Brand.SellerId == model.OrganisationId && x.IsActive);

            if (product is null)
            {
                throw new NotFoundException(_productLocalizer.GetString("ProductNotFound"));
            }

            if (await _context.Products.AnyAsync(x => x.PrimaryProductId == model.Id && x.Brand.SellerId == model.OrganisationId && x.IsActive))
            {
                throw new ConflictException(_productLocalizer.GetString("ProductVariantsDeleteProductConflict"));
            }

            product.LastModifiedDate = DateTime.UtcNow;
            product.IsActive = false;

            await _context.SaveChangesAsync();

            var message = new DeletedProductIntegrationEvent
            {
                ProductId = model.Id,
                Language = model.Language,
                Username = model.Username,
                OrganisationId = model.OrganisationId
            };

            using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {message.GetType().Name}");
            
            _eventBus.Publish(message);

            await _productIndexingRepository.IndexAsync(product.Id);
        }

        public async Task<PagedResults<IEnumerable<ProductServiceModel>>> GetAsync(GetProductsServiceModel model)
        {
            var searchResults = await _productSearchRepository.GetAsync(
                model.Language, 
                model.CategoryId, 
                model.OrganisationId, 
                model.HasPrimaryProduct,
                model.IsNew,
                model.IsSeller,
                model.SearchTerm,
                model.PageIndex, 
                model.ItemsPerPage,
                model.OrderBy);

            return await this.MapToPageResultsAsync(searchResults, model.Language, model.OrganisationId, model.IsSeller);
        }

        public async Task<PagedResults<IEnumerable<ProductServiceModel>>> GetByIdsAsync(GetProductsByIdsServiceModel model)
        {
            var searchResults = await _productSearchRepository.GetAsync(model.Language, model.OrganisationId, model.IsSeller, model.Ids, model.OrderBy);
            
            return await this.MapToPageResultsAsync(searchResults, model.Language, model.OrganisationId, model.IsSeller);
        }

        public async Task<ProductServiceModel> GetByIdAsync(GetProductByIdServiceModel model)
        {
            var searchResultItem = await _productSearchRepository.GetByIdAsync(model.Id.Value, model.Language, model.OrganisationId, model.IsSeller);

            if (searchResultItem is not null)
            {
                var productSearchModel = this.MapProductSearchModelToProductResult(searchResultItem);

                if (!searchResultItem.PrimaryProductIdHasValue)
                {
                    var productVariants = await _productSearchRepository.GetProductVariantsAsync(searchResultItem.ProductId, model.Language, model.OrganisationId, model.IsSeller);
                    productSearchModel.ProductVariants = productVariants?.Data?.Select(x => x.ProductId);
                }

                return productSearchModel;
            }

            return default;
        }

        public async Task<ProductServiceModel> GetBySkuAsync(GetProductBySkuServiceModel model)
        {
            var searchResultItem = await _productSearchRepository.GetBySkuAsync(model.Sku, model.Language, model.OrganisationId, model.IsSeller);

            if (searchResultItem is not null)
            {
                var productSearchModel = this.MapProductSearchModelToProductResult(searchResultItem);

                if (!searchResultItem.PrimaryProductIdHasValue)
                {
                    var productVariants = await _productSearchRepository.GetProductVariantsAsync(searchResultItem.ProductId, model.Language, model.OrganisationId, model.IsSeller);
                    productSearchModel.ProductVariants = productVariants?.Data?.Select(x => x.ProductId);
                }

                return productSearchModel;
            }

            return default;
        }

        public IEnumerable<string> GetProductSuggestions(GetProductSuggestionsServiceModel model)
        {
            return _productSearchRepository.GetProductSuggestions(model.SearchTerm, model.Size, model.Language, model.OrganisationId);
        }

        public async Task<PagedResults<IEnumerable<ProductServiceModel>>> GetBySkusAsync(GetProductsBySkusServiceModel model)
        {
            var products = await _productSearchRepository.GetAsync(model.Language, model.OrganisationId, model.IsSeller, model.Skus, model.OrderBy);

            return await this.MapToPageResultsAsync(products, model.Language, model.OrganisationId, model.IsSeller);
        }

        private async Task<PagedResults<IEnumerable<ProductServiceModel>>> MapToPageResultsAsync(PagedResults<IEnumerable<ProductSearchModel>> searchResults, string language, Guid? organisationId, bool? isSeller)
        {
            if (searchResults?.Data is not null && searchResults.Data.Any())
            {
                var products = new List<ProductServiceModel>();

                foreach (var searchResultItem in searchResults.Data)
                {
                    var productSearchModel = this.MapProductSearchModelToProductResult(searchResultItem);

                    if (!searchResultItem.PrimaryProductIdHasValue)
                    {
                        var productVariants = await _productSearchRepository.GetProductVariantsAsync(searchResultItem.ProductId, language, organisationId, isSeller);
                        
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

                if (productAttributeObject is not null)
                {
                    var productAttribute = new ProductAttributeServiceModel
                    { 
                        Key = productAttributeSearchModel.Key,
                        Name = productAttributeObject["name"].Value<string>()
                    };

                    if (productAttributeObject["value"].Type == JTokenType.Array)
                    {
                        var valuesArray = (JArray)productAttributeObject["value"];

                        if (valuesArray is not null)
                        {
                            productAttribute.Values = valuesArray.Children().OfType<JObject>().Select(o => o["name"]?.Value<string>()).Where(name => name != null);
                        }
                    }
                    else if (productAttributeObject["value"].Type == JTokenType.Object)
                    {
                        var valueObject = (JObject)productAttributeObject["value"];

                        if (valueObject is not null)
                        {
                            productAttribute.Values = new string[] { valueObject["name"].Value<string>() };
                        }
                    }
                    else if (productAttributeObject["value"].Type == JTokenType.Boolean)
                    {
                        if (productAttributeObject["value"].Value<bool>())
                        {
                            productAttribute.Values = new string[] { _globalLocalizer.GetString("Yes") };
                        }
                        else
                        {
                            productAttribute.Values = new string[] { _globalLocalizer.GetString("No") };
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
                PrimaryProductSku = searchResultItem.PrimaryProductSku,
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
                FulfillmentTime = searchResultItem.FulfillmentTime,
                Name = searchResultItem.Name,
                Description = searchResultItem.Description,
                FormData = searchResultItem.FormData,
                StockAvailableQuantity = searchResultItem.StockAvailableQuantity,
                OutletAvailableQuantity = searchResultItem.OutletAvailableQuantity,
                ProductAttributes = productAttributes,
                LastModifiedDate = searchResultItem.LastModifiedDate,
                CreatedDate = searchResultItem.CreatedDate
            };
        }

        public void TriggerCatalogIndexRebuild(RebuildCatalogIndexServiceModel model)
        {
            using var source = new ActivitySource(this.GetType().Name);

            var message = new RebuildCatalogSearchIndexIntegrationEvent
            {
                OrganisationId = model.OrganisationId,
                Language = model.Language,
                Username = model.Username
            };

            using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {message.GetType().Name}");
            
            _eventBus.Publish(message);
        }

        public async Task<PagedResults<IEnumerable<ProductFileServiceModel>>> GetProductFiles(GetProductFilesServiceModel model)
        {
            var productFiles = from f in _context.ProductFiles
                                              where f.ProductId == model.Id && f.IsActive
                                              select new ProductFileServiceModel
                                              {
                                                  Id = f.MediaId,
                                                  LastModifiedDate = f.LastModifiedDate,
                                                  CreatedDate = f.CreatedDate
                                              };

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                productFiles = productFiles.Where(x => x.Id.ToString() == model.SearchTerm);
            }

            productFiles = productFiles.ApplySort(model.OrderBy);

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                productFiles = productFiles.Take(Constants.MaxItemsPerPageLimit);

                return productFiles.PagedIndex(new Pagination(productFiles.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }

            return productFiles.PagedIndex(new Pagination(productFiles.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
        }

        public async Task<PagedResultsWithFilters<IEnumerable<ProductServiceModel>>> GetPagedResultsWithFilters(SearchProductsServiceModel model)
        {
            var searchResults = await _productSearchRepository.GetPagedResultsWithFilters(model.Language, model.OrganisationId, model.PageIndex, model.ItemsPerPage, model.OrderBy, model.Filters, model.IsSeller);

            var pageResullt = new PagedResults<IEnumerable<ProductSearchModel>>(searchResults.Total, searchResults.PageSize)
            {
                Data = searchResults.Data
            };

            var mappedPage = await MapToPageResultsAsync(pageResullt, model.Language, model.OrganisationId, model.IsSeller);

            return new PagedResultsWithFilters<IEnumerable<ProductServiceModel>>(searchResults.Total, searchResults.PageSize)
            {
                Data = mappedPage.Data,
                Filters = searchResults.Filters
            };
        }

        public async Task<PagedResultsWithFilters<IEnumerable<ProductServiceModel>>> GetPagedResultsWithFiltersByIds(SearchProductsByIdsServiceModel model)
        {
            var searchResults = await _productSearchRepository.GetPagedResultsWithFilters(model.Language, model.Ids, model.OrganisationId, model.OrderBy, model.Filters, model.IsSeller);

            var pageResullt = new PagedResults<IEnumerable<ProductSearchModel>>(searchResults.Total, searchResults.PageSize)
            {
                Data = searchResults.Data
            };

            var mappedPage = await MapToPageResultsAsync(pageResullt, model.Language, model.OrganisationId, model.IsSeller);

            return new PagedResultsWithFilters<IEnumerable<ProductServiceModel>>(searchResults.Total, searchResults.PageSize)
            {
                Data = mappedPage.Data,
                Filters = searchResults.Filters
            };
        }
    }
}
