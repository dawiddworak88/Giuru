using Buyer.Web.Areas.Products.Definitions;
using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Areas.Products.Repositories;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Shared.Definitions.Files;
using Buyer.Web.Shared.DomainModels.Media;
using Buyer.Web.Shared.Repositories.Media;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.Media.Services.MediaServices;
using Foundation.PageContent.Components.CarouselGrids.Definitions;
using Foundation.PageContent.Components.CarouselGrids.ViewModels;
using Foundation.PageContent.Components.Images;
using Foundation.PageContent.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class ProductsApiController : BaseApiController
    {
        private readonly IProductsService productsService;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly IProductsRepository productsRepository;
        private readonly IInventoryRepository inventoryRepository;
        private readonly IOutletRepository outletRepository;
        private readonly IMediaItemsRepository mediaRepository;
        private readonly IMediaService mediaService;
        private readonly LinkGenerator linkGenerator;

        public ProductsApiController(
            IProductsService productsService,
            IProductsRepository productsRepository,
            IStringLocalizer<ProductResources> productLocalizer,
            IMediaItemsRepository mediaRepository,
            IMediaService mediaService,
            IInventoryRepository inventoryRepository,
            IOutletRepository outletRepository,
            LinkGenerator linkGenerator)
        {
            this.productsService = productsService;
            this.productsRepository = productsRepository;
            this.linkGenerator = linkGenerator;
            this.productLocalizer = productLocalizer;
            this.mediaService = mediaService;
            this.productLocalizer = productLocalizer;
            this.inventoryRepository = inventoryRepository;
            this.outletRepository = outletRepository;
            this.mediaRepository = mediaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? categoryId, Guid? brandId, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var products = await this.productsService.GetProductsAsync(
                null,
                categoryId,
                brandId,
                CultureInfo.CurrentUICulture.Name,
                searchTerm,
                pageIndex,
                itemsPerPage,
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName));

            return this.StatusCode((int)HttpStatusCode.OK, products);
        }

        [HttpGet]
        public async Task<IActionResult> GetSuggestion(string searchTerm, Guid? brandId, bool? hasPrimaryProduct, int pageIndex, int itemsPerPage, string orderBy)
        {
            var language = CultureInfo.CurrentUICulture.Name;
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var products = await this.productsRepository.GetProductsAsync(
                token,
                language,
                searchTerm,
                hasPrimaryProduct,
                brandId,
                pageIndex,
                itemsPerPage,
                orderBy);

            return this.StatusCode((int)HttpStatusCode.OK, products);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductVariants(Guid? id)
        {
            var language = CultureInfo.CurrentUICulture.Name;
            var product = await this.productsRepository.GetProductAsync(id, language, null);

            if (product?.ProductVariants is not null)
            {
                var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
                var productVariants = await this.productsRepository.GetProductsAsync(
                    product.ProductVariants, null, null, language, null, PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize, token, $"{nameof(Product.Name)} ASC");

                var availableProducts = await this.inventoryRepository.GetAvailbleProductsInventory(
                    language, PaginationConstants.DefaultPageIndex, AvailableProductsConstants.Pagination.ItemsPerPage, null);

                var availableOutletProducts = await this.outletRepository.GetOutletProductsAsync(language, PaginationConstants.DefaultPageIndex, AvailableProductsConstants.Pagination.ItemsPerPage, token);

                var carouselItems = new List<CarouselGridCarouselItemViewModel>();
                foreach (var productVariant in productVariants.Data.OrEmptyIfNull())
                {
                    var carouselItem = new CarouselGridCarouselItemViewModel
                    {
                        Id = productVariant.Id,
                        Title = productVariant.Name,
                        Subtitle = productVariant.Sku,
                        Ean = productVariant.Ean,
                        ImageAlt = productVariant.Name,
                        Url = this.linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, productVariant.Id }),
                        ProductAttributes = await this.productsService.GetProductAttributesAsync(productVariant.ProductAttributes)
                    };

                    if (productVariant.Images != null && productVariant.Images.Any())
                    {
                        var variantImage = productVariant.Images.FirstOrDefault();
                        carouselItem.Sources = new List<SourceViewModel>
                        {
                            new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetMediaUrl(variantImage, 1024) },
                            new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetMediaUrl(variantImage, 352) },
                            new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetMediaUrl(variantImage, 608) },
                            new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetMediaUrl(variantImage, 768) },

                            new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetMediaUrl(variantImage, 1024) },
                            new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetMediaUrl(variantImage, 352) },
                            new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetMediaUrl(variantImage, 608) },
                            new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetMediaUrl(variantImage, 768) }
                        };

                        var variantImages = new List<ImageVariantViewModel>();
                        foreach (var image in productVariant.Images)
                        {
                            var imageVariantViewModel = new ImageVariantViewModel
                            {
                                Id = image
                            };
                            variantImages.Add(imageVariantViewModel);
                        }
                        carouselItem.Images = variantImages;
                        carouselItem.ImageUrl = this.mediaService.GetMediaUrl(variantImage, CarouselGridConstants.CarouselItemImageMaxWidth);
                    }

                    var availableProduct = availableProducts.Data.FirstOrDefault(x => x.ProductSku == productVariant.Sku);
                    if (availableProduct is not null)
                    {
                        carouselItem.AvailableQuantity = availableProduct.AvailableQuantity;
                        carouselItem.ExpectedDelivery = availableProduct.ExpectedDelivery;
                    }

                    var availableOutletProduct = availableOutletProducts.Data.FirstOrDefault(x => x.ProductSku == productVariant.Sku);
                    if (availableOutletProduct is not null)
                    {
                        carouselItem.AvailableOutletQuantity = availableOutletProduct.AvailableQuantity;
                        carouselItem.OutletTitle = availableOutletProduct.Title;
                    }

                    carouselItems.Add(carouselItem);
                }

                var response = new List<CarouselGridItemViewModel>
                {
                    new CarouselGridItemViewModel
                    {
                        Id = product.Id,
                        Title = this.productLocalizer.GetString("ProductVariants"),
                        CarouselItems = carouselItems
                    }
                };

                return this.StatusCode((int)HttpStatusCode.OK, response);
            }

            return this.StatusCode((int)HttpStatusCode.OK);
        }

        [HttpGet]
        public async Task<IActionResult> GetFiles(Guid? id, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var productFiles = await this.productsRepository.GetProductFilesAsync(token, language, id, pageIndex, itemsPerPage, searchTerm, $"{nameof(ProductFile.CreatedDate)} desc");

            var filesModel = new List<FileItem>();
            var filesIds = productFiles.Data.Select(x => x.Id);

            if (productFiles is not null && filesIds.Any())
            {
                var files = await this.mediaRepository.GetMediaItemsAsync(token, language, filesIds, FilesConstants.DefaultPageIndex, FilesConstants.DefaultPageSize);

                foreach (var file in files.OrEmptyIfNull())
                {
                    var fileModel = new FileItem
                    {
                        Id = file.Id,
                        Name = file.Name,
                        Filename = file.Filename,
                        Url = this.mediaService.GetNonCdnMediaUrl(file.Id),
                        Description = file.Description ?? "-",
                        IsProtected = file.IsProtected,
                        Size = this.mediaService.ConvertToMB(file.Size),
                        LastModifiedDate = file.LastModifiedDate,
                        CreatedDate = file.CreatedDate
                    };

                    filesModel.Add(fileModel);
                }
            }

            var pagedFiles = new PagedResults<IEnumerable<FileItem>>(filesModel.Count, FilesConstants.DefaultPageSize)
            {
                Data = filesModel
            };

            return this.StatusCode((int)HttpStatusCode.OK, pagedFiles);
        }
    } 
}