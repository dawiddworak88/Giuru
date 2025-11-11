using Buyer.Web.Areas.Products.ApiResponseModels;
using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Areas.Products.Repositories;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.Services.ProductColors;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Files;
using Buyer.Web.Shared.Definitions.Middlewares;
using Buyer.Web.Shared.DomainModels.Media;
using Buyer.Web.Shared.DomainModels.Prices;
using Buyer.Web.Shared.Repositories.Media;
using Buyer.Web.Shared.Services.Prices;
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
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Buyer.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class ProductsApiController : BaseApiController
    {
        private readonly IProductsService _productsService;
        private readonly IStringLocalizer<ProductResources> _productLocalizer;
        private readonly IProductsRepository _productsRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IOutletRepository _outletRepository;
        private readonly IMediaItemsRepository _mediaRepository;
        private readonly IMediaService _mediaService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IOptions<AppSettings> _options;
        private readonly IPriceService _priceService;
        private readonly IProductColorsService _productColorsService;
        private readonly ILogger<ProductsApiController> _logger;

        public ProductsApiController(
            IProductsService productsService,
            IProductsRepository productsRepository,
            IStringLocalizer<ProductResources> productLocalizer,
            IMediaItemsRepository mediaRepository,
            IMediaService mediaService,
            IInventoryRepository inventoryRepository,
            IOutletRepository outletRepository,
            IOptions<AppSettings> options,
            IPriceService priceService,
            LinkGenerator linkGenerator,
            IProductColorsService productColorsService,
            ILogger<ProductsApiController> logger)
        {
            _productsService = productsService;
            _productsRepository = productsRepository;
            _linkGenerator = linkGenerator;
            _productLocalizer = productLocalizer;
            _mediaService = mediaService;
            _productLocalizer = productLocalizer;
            _inventoryRepository = inventoryRepository;
            _outletRepository = outletRepository;
            _mediaRepository = mediaRepository;
            _options = options;
            _priceService = priceService;
            _productColorsService = productColorsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? categoryId, Guid? brandId, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var products = await _productsService.GetProductsAsync(
                null,
                categoryId,
                brandId,
                CultureInfo.CurrentUICulture.Name,
                searchTerm,
                false,
                pageIndex,
                itemsPerPage,
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName));

            return StatusCode((int)HttpStatusCode.OK, products);
        }

        [HttpGet]
        public async Task<IActionResult> GetSuggestion(string searchTerm, Guid? brandId, bool? hasPrimaryProduct, int pageIndex, int itemsPerPage, string orderBy)
        {
            var language = CultureInfo.CurrentUICulture.Name;
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var products = await _productsRepository.GetProductsAsync(
                token,
                language,
                searchTerm,
                hasPrimaryProduct,
                brandId,
                pageIndex,
                itemsPerPage,
                orderBy);

            return StatusCode((int)HttpStatusCode.OK, products);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductVariants(Guid? id)
        {
            var language = CultureInfo.CurrentUICulture.Name;
            var product = await _productsRepository.GetProductAsync(id, language, null);

            if (product?.ProductVariants is not null)
            {
                var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);

                var productVariants = await _productsRepository.GetProductsAsync(
                    product.ProductVariants, 
                    null, 
                    null, 
                    language, 
                    null, 
                    false, 
                    PaginationConstants.DefaultPageIndex, 
                    PaginationConstants.DefaultPageSize, 
                    token, 
                    $"{nameof(Product.Name)} ASC");

                var availableProducts = await _inventoryRepository.GetAvailbleProductsInventoryByIds(
                    token, 
                    language, 
                    productVariants.Data.OrEmptyIfNull().Select(x => x.Id));

                var availableOutletProducts = await _outletRepository.GetOutletProductsByIdsAsync(
                    token, 
                    language, 
                    productVariants.Data.OrEmptyIfNull().Select(x => x.Id));

                var carouselItems = new List<CarouselGridCarouselItemViewModel>();

                var prices = Enumerable.Empty<Price>();

                if (string.IsNullOrWhiteSpace(_options.Value.GrulaAccessToken) is false)
                {
                    var priceProducts = productVariants.Data.Select(async x => new PriceProduct
                    {
                        PrimarySku = x.PrimaryProductSku,
                        FabricsGroup = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossiblePriceGroupAttributeKeys),
                        ExtraPacking = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleExtraPackingAttributeKeys).ToYesOrNo(),
                        SleepAreaSize = _productsService.GetSleepAreaSize(x.ProductAttributes),
                        PaletteSize = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossiblePaletteSizeAttributeKeys),
                        Size = _productsService.GetSize(x.ProductAttributes),
                        PointsOfLight = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossiblePointsOfLightAttributeKeys),
                        LampshadeType = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleLampshadeTypeAttributeKeys),
                        LampshadeSize = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleLampshadeSizeAttributeKeys),
                        LinearLight = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleLinearLightAttributeKeys).ToYesOrNo(),
                        Mirror = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleMirrorAttributeKeys).ToYesOrNo(),
                        Shape = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleShapeAttributeKeys),
                        PrimaryColor = await _productColorsService.ToEnglishAsync(_productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossiblePrimaryColorAttributeKeys)),
                        SecondaryColor = await _productColorsService.ToEnglishAsync(_productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleSecondaryColorAttributeKeys)),
                        ShelfType = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleShelfTypeAttributeKeys),
                        IsStock = (availableProducts.FirstOrDefault(inv => inv.ProductSku == x.Sku)?.AvailableQuantity > 0).ToYesOrNo()
                    });

                    prices = await _priceService.GetPrices(
                        _options.Value.GrulaAccessToken,
                        DateTime.UtcNow,
                        await Task.WhenAll(priceProducts),
                        new PriceClient
                        {
                            Id = string.IsNullOrWhiteSpace(User.FindFirst(ClaimsEnrichmentConstants.ClientIdClaimType)?.Value) ? null : Guid.Parse(User.FindFirst(ClaimsEnrichmentConstants.ClientIdClaimType)?.Value),
                            Name = User.Identity?.Name,
                            CurrencyCode = User.FindFirst(ClaimsEnrichmentConstants.CurrencyClaimType)?.Value,
                            ExtraPacking = User.FindFirst(ClaimsEnrichmentConstants.ExtraPackingClaimType)?.Value,
                            PaletteLoading = User.FindFirst(ClaimsEnrichmentConstants.PaletteLoadingClaimType)?.Value,
                            Country = User.FindFirst(ClaimsEnrichmentConstants.CountryClaimType)?.Value,
                            DeliveryZipCode = User.FindFirst(ClaimsEnrichmentConstants.ZipCodeClaimType)?.Value
                        });
                }

                for (int i = 0; i < productVariants.Data.Count(); i++)
                {
                    var productVariant = productVariants.Data.ElementAtOrDefault(i);

                    if (productVariant is null)
                    {
                        continue;
                    }

                    var carouselItem = new CarouselGridCarouselItemViewModel
                    {
                        Id = productVariant.Id,
                        Title = productVariant.Name,
                        Subtitle = productVariant.Sku,
                        Ean = productVariant.Ean,
                        ImageAlt = productVariant.Name,
                        Url = _linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, productVariant.Id }),
                        ProductAttributes = await _productsService.GetProductAttributesAsync(productVariant.ProductAttributes)
                    };

                    if (productVariant.Images != null && productVariant.Images.Any())
                    {
                        var variantImage = productVariant.Images.FirstOrDefault();

                        carouselItem.Sources = new List<SourceViewModel>
                        {
                            new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = _mediaService.GetMediaUrl(variantImage, 1024) },
                            new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = _mediaService.GetMediaUrl(variantImage, 352) },
                            new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = _mediaService.GetMediaUrl(variantImage, 608) },
                            new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = _mediaService.GetMediaUrl(variantImage, 768) },

                            new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = _mediaService.GetMediaUrl(variantImage, 1024) },
                            new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = _mediaService.GetMediaUrl(variantImage, 352) },
                            new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = _mediaService.GetMediaUrl(variantImage, 608) },
                            new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = _mediaService.GetMediaUrl(variantImage, 768) }
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
                        carouselItem.ImageUrl = _mediaService.GetMediaUrl(variantImage, CarouselGridConstants.CarouselItemImageMaxWidth);
                    }

                    var availableProduct = availableProducts.FirstOrDefault(x => x.ProductSku == productVariant.Sku);

                    if (availableProduct is not null)
                    {
                        carouselItem.InStock = true;
                        carouselItem.AvailableQuantity = availableProduct.AvailableQuantity;
                        carouselItem.ExpectedDelivery = availableProduct.ExpectedDelivery;
                    }

                    var availableOutletProduct = availableOutletProducts.FirstOrDefault(x => x.ProductSku == productVariant.Sku);

                    if (availableOutletProduct is not null)
                    {
                        carouselItem.InOutlet = true;
                        carouselItem.AvailableOutletQuantity = availableOutletProduct.AvailableQuantity;
                        carouselItem.OutletTitle = availableOutletProduct.Title;
                    }

                    if (prices.Any())
                    {
                        var price = prices.ElementAtOrDefault(i);

                        if (price is not null)
                        {
                            carouselItem.Price = new CarouselGridPriceViewModel
                            {
                                Current = price.CurrentPrice,
                                Currency = price.CurrencyCode
                            };
                        }
                    }

                    carouselItems.Add(carouselItem);
                }

                var response = new List<CarouselGridItemViewModel>
                {
                    new CarouselGridItemViewModel
                    {
                        Id = product.Id,
                        Title = _productLocalizer.GetString("ProductVariants"),
                        CarouselItems = carouselItems
                    }
                };

                return StatusCode((int)HttpStatusCode.OK, response);
            }

            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpGet]
        public async Task<IActionResult> GetFiles(Guid? id, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var productFiles = await _productsRepository.GetProductFilesAsync(token, language, id, pageIndex, itemsPerPage, searchTerm, $"{nameof(ProductFile.CreatedDate)} desc");

            var filesModel = new List<FileItem>();
            var filesIds = productFiles.Data.Select(x => x.Id);

            if (productFiles is not null && filesIds.Any())
            {
                var files = await _mediaRepository.GetMediaItemsAsync(token, language, filesIds, FilesConstants.DefaultPageIndex, FilesConstants.DefaultPageSize);

                foreach (var file in files.OrEmptyIfNull())
                {
                    var fileModel = new FileItem
                    {
                        Id = file.Id,
                        Name = file.Name,
                        Filename = file.Filename,
                        Url = _mediaService.GetNonCdnMediaUrl(file.Id),
                        Description = file.Description ?? "-",
                        IsProtected = file.IsProtected,
                        Size = _mediaService.ConvertToMB(file.Size),
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

            return StatusCode((int)HttpStatusCode.OK, pagedFiles);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsQuantities(string searchTerm, int pageIndex, bool? hasPrimaryProduct, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var products = await _productsRepository.GetProductsAsync(
                token,
                language,
                searchTerm,
                hasPrimaryProduct,
                null,
                pageIndex,
                itemsPerPage,
                null);

            if (products.Data.Any())
            {
                var prices = Enumerable.Empty<Price>();

                var inventories = await _inventoryRepository.GetAvailbleProductsByProductIdsAsync(
                    token,
                    language,
                    products.Data.Select(x => x.Id));

                if (string.IsNullOrWhiteSpace(_options.Value.GrulaAccessToken) is false)
                {
                    var priceProducts = products.Data.Select(async x => new PriceProduct
                    {
                        PrimarySku = x.PrimaryProductSku,
                        FabricsGroup = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossiblePriceGroupAttributeKeys),
                        ExtraPacking = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleExtraPackingAttributeKeys),
                        SleepAreaSize = _productsService.GetSleepAreaSize(x.ProductAttributes),
                        PaletteSize = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossiblePaletteSizeAttributeKeys),
                        Size = _productsService.GetSize(x.ProductAttributes),
                        PointsOfLight = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossiblePointsOfLightAttributeKeys),
                        LampshadeType = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleLampshadeTypeAttributeKeys),
                        LampshadeSize = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleLampshadeSizeAttributeKeys),
                        LinearLight = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleLinearLightAttributeKeys).ToYesOrNo(),
                        Mirror = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleMirrorAttributeKeys).ToYesOrNo(),
                        Shape = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleShapeAttributeKeys),
                        PrimaryColor = await _productColorsService.ToEnglishAsync(_productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossiblePrimaryColorAttributeKeys)),
                        SecondaryColor = await _productColorsService.ToEnglishAsync(_productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleSecondaryColorAttributeKeys)),
                        ShelfType = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleShelfTypeAttributeKeys),
                        IsStock = (inventories.FirstOrDefault(inv => inv.ProductId == x.Id)?.AvailableQuantity > 0).ToYesOrNo()
                    });

                    prices = await _priceService.GetPrices(
                        _options.Value.GrulaAccessToken,
                        DateTime.UtcNow,
                        await Task.WhenAll(priceProducts),
                        new PriceClient
                        {
                            Id = string.IsNullOrWhiteSpace(User.FindFirst(ClaimsEnrichmentConstants.ClientIdClaimType)?.Value) ? null : Guid.Parse(User.FindFirst(ClaimsEnrichmentConstants.ClientIdClaimType)?.Value),
                            Name = User.Identity?.Name,
                            CurrencyCode = User.FindFirst(ClaimsEnrichmentConstants.CurrencyClaimType)?.Value,
                            ExtraPacking = User.FindFirst(ClaimsEnrichmentConstants.ExtraPackingClaimType)?.Value,
                            PaletteLoading = User.FindFirst(ClaimsEnrichmentConstants.PaletteLoadingClaimType)?.Value,
                            Country = User.FindFirst(ClaimsEnrichmentConstants.CountryClaimType)?.Value,
                            DeliveryZipCode = User.FindFirst(ClaimsEnrichmentConstants.ZipCodeClaimType)?.Value
                        });
                }

                var outlets = await _outletRepository.GetOutletProductsByProductsIdAsync(
                    token,
                    language,
                    products.Data.Select(x => x.Id));

                var productsQuantities = new List<ProductQuantitiesResponseModel>();

                for (int i = 0; i < products.Data.Count(); i++)
                {
                    var product = products.Data.ElementAtOrDefault(i);

                    if (product is null)
                    {
                        continue;
                    }

                    var productQuantity = new ProductQuantitiesResponseModel
                    {
                        Id = product.Id,
                        Sku = product.Sku,
                        Name = product.Name,
                        Images = product.Images,
                        StockQuantity = inventories.FirstOrDefault(y => y.ProductId == product.Id)?.AvailableQuantity ?? 0,
                        OutletQuantity = outlets.FirstOrDefault(y => y.ProductId == product.Id)?.AvailableQuantity ?? 0,
                    };

                    if (prices.Any())
                    {
                        var price = prices.ElementAtOrDefault(i);

                        if (price is not null)
                        {
                            productQuantity.Price = price.CurrentPrice;
                            productQuantity.Currency = price.CurrencyCode;
                        }
                    }

                    productsQuantities.Add(productQuantity);
                }

                return StatusCode((int)HttpStatusCode.OK, productsQuantities);
            }

            return StatusCode((int)HttpStatusCode.OK, Enumerable.Empty<ProductQuantitiesResponseModel>());
        }

        [HttpGet]
        public async Task<IActionResult> GetPrice(string sku)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var product = await _productsRepository.GetProductAsync(sku, language, token);

            if (string.IsNullOrWhiteSpace(_options.Value.GrulaAccessToken) is false)
            {
                var outletItem = await _outletRepository.GetOutletProductBySkuAsync(token, language, sku);

                try
                {
                    var price = await _priceService.GetPrice(
                    _options.Value.GrulaAccessToken,
                    DateTime.UtcNow,
                    new PriceProduct
                    {
                        PrimarySku = product.PrimaryProductSku,
                        FabricsGroup = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossiblePriceGroupAttributeKeys),
                        ExtraPacking = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleExtraPackingAttributeKeys).ToYesOrNo(),
                        SleepAreaSize = _productsService.GetSleepAreaSize(product.ProductAttributes),
                        PaletteSize = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossiblePaletteSizeAttributeKeys),
                        Size = _productsService.GetSize(product.ProductAttributes),
                        PointsOfLight = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossiblePointsOfLightAttributeKeys),
                        LampshadeType = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleLampshadeTypeAttributeKeys),
                        LampshadeSize = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleLampshadeSizeAttributeKeys),
                        LinearLight = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleLinearLightAttributeKeys).ToYesOrNo(),
                        Mirror = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleMirrorAttributeKeys).ToYesOrNo(),
                        Shape = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleShapeAttributeKeys),
                        PrimaryColor = await _productColorsService.ToEnglishAsync(_productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossiblePrimaryColorAttributeKeys)),
                        SecondaryColor = await _productColorsService.ToEnglishAsync(_productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleSecondaryColorAttributeKeys)),
                        ShelfType = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleShelfTypeAttributeKeys),
                        IsOutlet = (outletItem?.AvailableQuantity > 0).ToYesOrNo(),
                        IsStock = ((await _inventoryRepository.GetAvailbleProductsByProductIdsAsync(token, language, [product.Id])).FirstOrDefault().AvailableQuantity > 0).ToYesOrNo()
                    },
                    new PriceClient
                    {
                        Id = string.IsNullOrWhiteSpace(User.FindFirst(ClaimsEnrichmentConstants.ClientIdClaimType)?.Value) ? null : Guid.Parse(User.FindFirst(ClaimsEnrichmentConstants.ClientIdClaimType)?.Value),
                        Name = User.Identity?.Name,
                        CurrencyCode = User.FindFirst(ClaimsEnrichmentConstants.CurrencyClaimType)?.Value,
                        ExtraPacking = User.FindFirst(ClaimsEnrichmentConstants.ExtraPackingClaimType)?.Value,
                        PaletteLoading = User.FindFirst(ClaimsEnrichmentConstants.PaletteLoadingClaimType)?.Value,
                        Country = User.FindFirst(ClaimsEnrichmentConstants.CountryClaimType)?.Value,
                        DeliveryZipCode = User.FindFirst(ClaimsEnrichmentConstants.ZipCodeClaimType)?.Value
                    });

                    if (price is not null)
                    {
                        return StatusCode((int)HttpStatusCode.OK, new PriceResponseModel
                        {
                            CurrencyCode = price.CurrencyCode,
                            CurrentPrice = price.CurrentPrice
                        });
                    }
                }
                catch
                {
                    return StatusCode((int)HttpStatusCode.OK);
                }
            }

            return StatusCode((int)HttpStatusCode.OK);
        }
    } 
}
