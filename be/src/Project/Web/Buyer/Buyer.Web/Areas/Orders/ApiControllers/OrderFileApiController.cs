using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using Buyer.Web.Areas.Orders.Definitions;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Orders.Services.Basket;
using Buyer.Web.Areas.Orders.Services.OrderFiles;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Areas.Products.Services.ProductColors;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Basket;
using Buyer.Web.Shared.Definitions.Files;
using Buyer.Web.Shared.DomainModels.Media;
using Buyer.Web.Shared.Extensions;
using Buyer.Web.Shared.Repositories.Inventory;
using Buyer.Web.Shared.Repositories.Media;
using Foundation.Pricing.Baskets;
using Foundation.Pricing.Services;
using Buyer.Web.Shared.Services.Prices;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Exceptions;
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
        private readonly IPriceProductFactory _priceProductFactory;
        private readonly IPriceClientResolver _priceClientResolver;
        private readonly IBasketRepricingService _basketRepricingService;

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
            IProductColorsService productColorsService,
            IPriceProductFactory priceProductFactory,
            IPriceClientResolver priceClientResolver,
            IBasketRepricingService basketRepricingService)
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
            _priceProductFactory = priceProductFactory;
            _priceClientResolver = priceClientResolver;
            _basketRepricingService = basketRepricingService;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] UploadMediaRequestModel model)
        {
            var importedOrderLines = OrderBasketUploadHelper.GroupImportedLines(_orderFileService.ImportOrderLines(model.File));
            var basketItems = new List<BasketItem>();

            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;
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
            var existingBasket = await _basketRepository.GetBasketById(token, language, id);
            var canSeePrices = _priceService.CanSeePrices(User.GetClientId());

            var importedSkus = importedOrderLines.OrEmptyIfNull().Select(x => x.Sku).Distinct().ToList();
            var skusParam = importedSkus
                .Concat(existingBasket?.Items.OrEmptyIfNull().Select(x => x.ProductSku) ?? Enumerable.Empty<string>())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct();
            var products = await _productsRepository.GetProductsBySkusAsync(token, language, skusParam);

            if (products.OrEmptyIfNull().Any(x => importedSkus.Contains(x.Sku)) is false)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new { Message = _orderLocalizer.GetString("ProductsNotFound").Value });
            }

            var productLookup = OrderBasketUploadHelper.CreateProductLookup(products);

            // One shared prologue with BasketsApiController: the same request must resolve to the same
            // code whichever entry point writes the basket. Past the ProductsNotFound guard at least one
            // imported line resolved to a product, so the merged basket always has lines - hasItems is
            // an invariant here, not a test, which makes the rejection below unreachable from this
            // entry point. It is kept rather than dropped so that weakening that guard cannot silently
            // start persisting a discount code against a basket with no lines.
            var discountOutcome = await BasketDiscountCodeCoordinator.ResolveAsync(
                _options.Value.IsGrulaConfigured,
                Request.Form.ContainsKey(nameof(UploadMediaRequestModel.DiscountCode)),
                model.DiscountCode,
                hasItems: true,
                () => Task.FromResult(existingBasket?.DiscountCode),
                () => _orderLocalizer.GetString("DiscountCodeRequiresBasketItems").Value);

            if (discountOutcome.IsRejected)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new { Message = discountOutcome.RejectionMessage });
            }

            var discountCode = discountOutcome.DiscountCode;

            var productIds = products.OrEmptyIfNull().Select(x => x.Id).Distinct();
            var stockAvailableProducts = await _inventoryRepository.GetStockAvailbleProductsByProductIdsAsync(token, language, productIds);

            var stockByProductId = stockAvailableProducts
                .OrEmptyIfNull()
                .ToDictionary(g => g.ProductId, g => (double)g.AvailableQuantity);

            OrderBasketUploadHelper.DeductExistingStock(stockByProductId, existingBasket?.Items);

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
                    ExternalReference = OrderBasketUploadHelper.NormalizeKeyPart(orderLine.ExternalReference),
                    MoreInfo = OrderBasketUploadHelper.NormalizeKeyPart(orderLine.MoreInfo)
                };

                basketItems.Add(basketItem);
            }

            var completeBasketItems = OrderBasketUploadHelper.Merge(existingBasket?.Items, basketItems).ToList();

            // Every price on every saved line is derived from one pass over this exact persisted line set.
            if (!_options.Value.IsGrulaConfigured)
            {
                BasketPriceApplier.ClearUntrustedPrices(completeBasketItems);
            }
            else
            {
                await RepriceMergedBasketAsync(token, language, completeBasketItems, productLookup, discountCode);
            }

            if (!canSeePrices)
            {
                // Defence in depth: price visibility must also be enforced immediately before saving.
                foreach (var item in completeBasketItems.OrEmptyIfNull())
                {
                    item.UnitPrice = null;
                    item.Price = null;
                    item.Currency = null;
                }
            }

            var basket = await _basketRepository.SaveAsync(token, language, id, completeBasketItems, discountCode);

            var basketResponseModel = new BasketResponseModel
            {
                Id = basket.Id,
                DiscountCode = basket.DiscountCode
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
                    UnitPrice = canSeePrices ? x.UnitPrice : null,
                    Price = canSeePrices ? x.Price : null,
                    Currency = canSeePrices ? x.Currency : null,
                    ExpectedLeadTime = x.ExpectedLeadTime
                });
            }

            return StatusCode((int)HttpStatusCode.OK, basketResponseModel);
        }

        private async Task RepriceMergedBasketAsync(
            string token,
            string language,
            IList<BasketItem> basketItems,
            IReadOnlyDictionary<string, Product> productLookup,
            string discountCode)
        {
            foreach (var item in basketItems.Where(x => string.IsNullOrWhiteSpace(x.ProductSku) || !productLookup.ContainsKey(x.ProductSku)))
            {
                _logger.LogWarning("Basket line SKU {Sku} could not be resolved to a product for language {Language}.", item.ProductSku, language);
            }

            var priceClient = await _priceClientResolver.ResolveAsync(null, discountCode, token);

            var outcome = await _basketRepricingService.RepriceAsync(
                basketItems,
                x => x.ProductSku,
                x => x.OutletQuantity > 0,
                productLookup,
                (product, isOutletPurchase) => _priceProductFactory.CreateAsync(product, isOutletPurchase),
                priceClient,
                DateTime.UtcNow);

            if (!outcome.Succeeded)
            {
                throw new CustomException(_orderLocalizer.GetString("BasketPricesCouldNotBeVerified").Value, (int)HttpStatusCode.UnprocessableEntity);
            }
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
