using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using Buyer.Web.Areas.Orders.Definitions;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Orders.Services.OrderFiles;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.Services.ProductColors;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Basket;
using Buyer.Web.Shared.Definitions.Files;
using Buyer.Web.Shared.Definitions.Middlewares;
using Buyer.Web.Shared.DomainModels.Media;
using Buyer.Web.Shared.DomainModels.Prices;
using Buyer.Web.Shared.Repositories.Inventory;
using Buyer.Web.Shared.Repositories.Media;
using Buyer.Web.Shared.Services.Prices;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.Media.Services.MediaServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    [Authorize]
    public class OrderFileApiController : BaseApiController
    {
        private readonly IOrderFileService _orderFileService;
        private readonly IProductsRepository _productsRepository;
        private readonly IBasketRepository _basketRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IOptions<AppSettings> _options;
        private readonly IMediaService _mediaService;
        private readonly ILogger<OrderFileApiController> _logger;
        private readonly IMediaItemsRepository _mediaRepository;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IPriceService _priceService;
        private readonly IProductsService _productsService;
        private readonly IProductColorsService _productColorsService;

        public OrderFileApiController(
            IOrderFileService orderFileService,
            IProductsRepository productsRepository,
            IBasketRepository basketRepository,
            LinkGenerator linkGenerator,
            IOptions<AppSettings> options,
            IMediaService mediaService,
            IMediaItemsRepository mediaRepository,
            IOrdersRepository ordersRepository,
            IInventoryRepository inventoryRepository,
            ILogger<OrderFileApiController> logger,
            IStringLocalizer<OrderResources> orderLocalizer,
            IPriceService priceService,
            IProductsService productsService,
            IProductColorsService productColorsService)
        {
            _orderFileService = orderFileService;
            _productsRepository = productsRepository;
            _basketRepository = basketRepository;
            _linkGenerator = linkGenerator;
            _options = options;
            _mediaService = mediaService;
            _logger = logger;
            _mediaRepository = mediaRepository;
            _ordersRepository = ordersRepository;
            _inventoryRepository = inventoryRepository;
            _orderLocalizer = orderLocalizer;
            _priceService = priceService;
            _productsService = productsService;
            _productColorsService = productColorsService;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] UploadMediaRequestModel model)
        {
            var importedOrderLines = _orderFileService.ImportOrderLines(model.File);
            var basketItems = new List<BasketItem>();

            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var skusParam = importedOrderLines.OrEmptyIfNull().Select(x => x.Sku).Distinct();
            var products = await _productsRepository.GetProductsBySkusAsync(token, language, skusParam);

            if (products.OrEmptyIfNull().Any() is false)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new { Message = _orderLocalizer.GetString("ProductsNotFound") });
            }

            var productLookup = products.ToDictionary(p => p.Sku);

            var orderedProducts = importedOrderLines
                .Where(x => productLookup.ContainsKey(x.Sku))
                .Select(x => productLookup[x.Sku]);

            var productIds = products.OrEmptyIfNull().Select(x => x.Id).Distinct();
            var stockAvailableProducts = await _inventoryRepository.GetStockAvailbleProductsByProductIdsAsync(token, language, productIds);

            var stockByProductId = stockAvailableProducts
                .OrEmptyIfNull()
                .ToDictionary(g => g.ProductId, g => (double)g.AvailableQuantity);

            var prices = Enumerable.Empty<Price>();

            if (string.IsNullOrWhiteSpace(_options.Value.GrulaAccessToken) is false)
            {
                var priceProducts = orderedProducts.Select(async x => new PriceProduct
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
                    ShelfType = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleShelfTypeAttributeKeys)
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

            var priceIndex = 0;

            foreach (var orderLine in importedOrderLines)
            {
                if (!productLookup.TryGetValue(orderLine.Sku, out var product))
                {
                    _logger.LogWarning($"Product for SKU {orderLine.Sku} and language {language} not found.");
                    continue;
                }

                var availableStock = stockByProductId.TryGetValue(product.Id, out var qty) ? qty : 0;

                var stockQuantity = Math.Min(orderLine.Quantity, availableStock);
                var quantity = orderLine.Quantity - stockQuantity;

                if (stockByProductId.ContainsKey(product.Id))
                {
                    stockByProductId[product.Id] = Math.Max(0, availableStock - stockQuantity);
                }

                var firstImage = product.Images.OrEmptyIfNull().FirstOrDefault();
                var pictureUrl = firstImage != Guid.Empty
                    ? _mediaService.GetMediaUrl(firstImage, OrdersConstants.Basket.BasketProductImageMaxWidth)
                    : null;

                var basketItem = new BasketItem
                {
                    ProductId = product.Id,
                    ProductSku = orderLine.Sku,
                    ProductName = product.Name,
                    PictureUrl = pictureUrl,
                    Quantity = quantity,
                    StockQuantity = stockQuantity,
                    ExternalReference = orderLine.ExternalReference,
                    MoreInfo = orderLine.MoreInfo
                };

                if (prices.Any())
                {
                    var price = prices.ElementAtOrDefault(priceIndex);

                    if (price is not null)
                    {
                        basketItem.UnitPrice = price.CurrentPrice;
                        basketItem.Price = price.CurrentPrice * (decimal)orderLine.Quantity;
                        basketItem.Currency = price.CurrencyCode;
                    }
                }

                basketItems.Add(basketItem);
                priceIndex++;
            }

            var reqCookie = Request.Cookies[BasketConstants.BasketCookieName];

            if (reqCookie is null)
            {
                reqCookie = Guid.NewGuid().ToString();

                var cookieOptions = new CookieOptions
                {
                    MaxAge = TimeSpan.FromDays(BasketConstants.BasketCookieMaxAge)
                };
                Response.Cookies.Append(BasketConstants.BasketCookieName, reqCookie, cookieOptions);
            }

            var id = Guid.Parse(reqCookie);

            var basket = await _basketRepository.SaveAsync(token, language, id, basketItems);

            var basketResponseModel = new BasketResponseModel
            {
                Id = basket.Id
            };

            if (basket.Items.OrEmptyIfNull().Any())
            {
                basketResponseModel.Items = basket.Items.OrEmptyIfNull().Select(x => new BasketItemResponseModel
                {
                    ProductId = x.ProductId,
                    ProductUrl = _linkGenerator.GetPathByAction("Edit", "Product", new { Area = "Products", culture = language, Id = x.ProductId }),
                    Name = x.ProductName,
                    Sku = x.ProductSku,
                    Quantity = x.Quantity,
                    StockQuantity = x.StockQuantity,
                    OutletQuantity = x.OutletQuantity,
                    ExternalReference = x.ExternalReference,
                    ImageSrc = x.PictureUrl,
                    ImageAlt = x.ProductName,
                    MoreInfo = x.MoreInfo,
                    UnitPrice = x.UnitPrice,
                    Price = x.Price,
                    Currency = x.Currency,
                });
            }

            return StatusCode((int)HttpStatusCode.OK, basketResponseModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetFiles(Guid? id, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var productFiles = await _ordersRepository.GetOrderFilesAsync(token, language, id, pageIndex, itemsPerPage, searchTerm, $"{nameof(OrderFile.CreatedDate)} desc");

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
    }
}
