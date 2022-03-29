using Buyer.Web.Areas.Products.Definitions;
using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Services.ContentDeliveryNetworks;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Services.MediaServices;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.PageContent.Components.CarouselGrids.Definitions;
using Foundation.PageContent.Components.CarouselGrids.ViewModels;
using Foundation.PageContent.Components.Images;
using Foundation.PageContent.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
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
        private readonly IOptions<AppSettings> options;
        private readonly ICdnService cdnService;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly IProductsRepository productsRepository;
        private readonly IInventoryRepository inventoryRepository;
        private readonly IMediaHelperService mediaService;
        private readonly LinkGenerator linkGenerator;

        public ProductsApiController(
            IProductsService productsService,
            IProductsRepository productsRepository,
            ICdnService cdnService,
            IStringLocalizer<ProductResources> productLocalizer,
            IMediaHelperService mediaService,
            IOptions<AppSettings> options,
            IInventoryRepository inventoryRepository,
            LinkGenerator linkGenerator)
        {
            this.productsService = productsService;
            this.productsRepository = productsRepository;
            this.linkGenerator = linkGenerator;
            this.productLocalizer = productLocalizer;
            this.mediaService = mediaService;
            this.productLocalizer = productLocalizer;
            this.options = options;
            this.cdnService = cdnService;
            this.inventoryRepository = inventoryRepository;
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

                var carouselItems = new List<CarouselGridCarouselItemViewModel>();
                foreach (var productVariant in productVariants.Data.OrEmptyIfNull())
                {
                    var carouselItem = new CarouselGridCarouselItemViewModel
                    {
                        Id = productVariant.Id,
                        Title = productVariant.Name,
                        Subtitle = productVariant.Sku,
                        ImageAlt = productVariant.Name,
                        Url = this.linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, productVariant.Id }),
                        ProductAttributes = await this.productsService.GetProductAttributesAsync(productVariant.ProductAttributes)
                    };

                    if (productVariant.Images != null && productVariant.Images.Any())
                    {
                        var variantImage = productVariant.Images.FirstOrDefault();
                        carouselItem.Sources = new List<SourceViewModel>
                        {
                            new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, variantImage, 1024, 1024, true, MediaConstants.WebpExtension)) },
                            new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, variantImage, 352, 352, true,MediaConstants.WebpExtension)) },
                            new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, variantImage, 608, 608, true, MediaConstants.WebpExtension)) },
                            new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, variantImage, 768, 768, true, MediaConstants.WebpExtension)) },

                            new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, variantImage, 1024, 1024, true)) },
                            new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, variantImage, 352, 352, true)) },
                            new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, variantImage, 608, 608, true)) },
                            new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, variantImage, 768, 768, true)) }
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
                        carouselItem.ImageUrl = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, variantImage, CarouselGridConstants.CarouselItemImageMaxWidth, CarouselGridConstants.CarouselItemImageMaxHeight, true));
                    }

                    var availableProduct = availableProducts.Data.FirstOrDefault(x => x.ProductSku == productVariant.Sku);
                    if (availableProduct is not null)
                    {
                        carouselItem.AvailableQuantity = availableProduct.AvailableQuantity;
                        carouselItem.ExpectedDelivery = availableProduct.ExpectedDelivery;
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
    } 
}