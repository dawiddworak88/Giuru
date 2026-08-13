using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using Buyer.Web.Areas.Orders.Definitions;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.Services.ProductColors;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Basket;
using Buyer.Web.Shared.Definitions.Middlewares;
using Buyer.Web.Shared.DomainModels.Prices;
using Buyer.Web.Shared.Extensions;
using Buyer.Web.Shared.Services.Baskets;
using Buyer.Web.Shared.Services.Prices;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
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
    public class BasketsApiController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMediaService _mediaService;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IProductsRepository _productsRepository;
        private readonly IPriceService _priceService;
        private readonly IProductsService _productsService;
        private readonly IProductColorsService _productColorsService;
        private readonly IOptions<AppSettings> _options;
        private readonly ILogger<BasketsApiController> _logger;

        public BasketsApiController(
            IBasketRepository basketRepository,
            LinkGenerator linkGenerator,
            IMediaService mediaService,
            IStringLocalizer<OrderResources> orderLocalizer,
            IProductsRepository productsRepository,
            IPriceService priceService,
            IProductsService productsService,
            IProductColorsService productColorsService,
            IOptions<AppSettings> options,
            ILogger<BasketsApiController> logger)
        {
            _basketRepository = basketRepository;
            _linkGenerator = linkGenerator;
            _mediaService = mediaService;
            _orderLocalizer = orderLocalizer;
            _productsRepository = productsRepository;
            _priceService = priceService;
            _productsService = productsService;
            _productColorsService = productColorsService;
            _options = options;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveBasketRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;
            var items = model.Items.OrEmptyIfNull().ToList();
            var clientId = User.GetClientId();

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
            string discountCode;

            if (_options.Value.IsGrulaConfigured)
            {
                var existingBasket = DiscountCodeResolver.RequiresPersistedDiscountCode(model.HasDiscountCode, model.DiscountCode, items.Any())
                    ? await _basketRepository.GetBasketById(token, language, id)
                    : null;
                var discount = DiscountCodeResolver.Resolve(model.HasDiscountCode, model.DiscountCode, existingBasket?.DiscountCode, items.Any());
                discountCode = discount.DiscountCode;

                if (discount.IsAppliedToEmptyBasket)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { Message = _orderLocalizer.GetString("DiscountCodeRequiresBasketItems").Value });
                }
            }
            else
            {
                // Discount codes are Grula price drivers. While pricing is unavailable,
                // ignore incoming mutations but retain any code already stored on the basket.
                var existingBasket = await _basketRepository.GetBasketById(token, language, id);
                discountCode = existingBasket?.DiscountCode;
            }

            if (_options.Value.IsGrulaConfigured && items.Any())
            {
                await RepriceBasketItemsAsync(token, language, items, discountCode);
            }
            else if (items.Any())
            {
                ClearUntrustedPrices(items);
            }

            var basket = await _basketRepository.SaveAsync(token, language, id,
                items.Select(x => new BasketItem
                {
                    ProductId = x.ProductId,
                    ProductSku = x.Sku,
                    ProductName = x.Name,
                    PictureUrl = !string.IsNullOrWhiteSpace(x.ImageSrc) ? x.ImageSrc : (x.ImageId.HasValue ? _mediaService.GetMediaUrl(x.ImageId.Value, OrdersConstants.Basket.BasketProductImageMaxWidth) : null),
                    Quantity = x.Quantity,
                    StockQuantity = x.StockQuantity,
                    OutletQuantity = x.OutletQuantity,
                    ExternalReference = x.ExternalReference,
                    MoreInfo = x.MoreInfo,
                    UnitPrice = x.UnitPrice,
                    Price = x.Price,
                    Currency = x.Currency,
                    ExpectedLeadTime = x.ExpectedLeadTime
                }),
                discountCode);

            var basketResponseModel = new BasketResponseModel
            {
                Id = basket.Id,
                DiscountCode = basket.DiscountCode
            };

            var productIds = basket.Items.OrEmptyIfNull().Select(x => x.ProductId.Value);
            if (productIds.OrEmptyIfNull().Any())
            {
                var canSeePrices = _priceService.CanSeePrices(clientId);

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

        internal static string GetOutletPriceDriver(BasketItemRequestModel basketItem)
        {
            return (basketItem.OutletQuantity > 0).ToYesOrNo();
        }

        internal static void ClearUntrustedPrices(IList<BasketItemRequestModel> basketItems)
        {
            BasketPriceApplier.ClearUntrustedPrices(basketItems);
        }

        internal static bool ApplyPrices(
            IList<BasketItemRequestModel> basketItems,
            IList<PriceLookupResult> priceResults)
        {
            return BasketPriceApplier.ApplyPrices(basketItems, priceResults);
        }

        // Grula returns exactly one outcome per supplied product, in the order the products were sent
        // (IPriceService.GetPriceResultsForBasketAsync). A response that breaks that contract cannot be
        // mapped back onto basket lines, so refuse to align it at all - the caller then persists the whole
        // basket unpriced rather than keeping prices for the subset that happened to come back.
        internal static IList<PriceLookupResult> AlignPrices(
            int basketItemCount,
            IList<int> pricedLineIndexes,
            IList<PriceLookupResult> prices)
        {
            return BasketPriceApplier.AlignPrices(basketItemCount, pricedLineIndexes, prices);
        }

        private async Task RepriceBasketItemsAsync(
            string token,
            string language,
            IList<BasketItemRequestModel> basketItems,
            string discountCode)
        {
            var skus = basketItems
                .Select(x => x.Sku)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct();
            var products = await _productsRepository.GetProductsBySkusAsync(token, language, skus);
            var productLookup = products.OrEmptyIfNull()
                .Where(x => !string.IsNullOrWhiteSpace(x.Sku))
                .GroupBy(x => x.Sku)
                .ToDictionary(x => x.Key, x => x.First());

            var unresolvedSkus = basketItems
                .Select(x => x.Sku)
                .Where(x => string.IsNullOrWhiteSpace(x) || !productLookup.ContainsKey(x))
                .Distinct()
                .ToList();

            foreach (var sku in unresolvedSkus)
            {
                _logger.LogWarning("Basket line SKU {Sku} could not be resolved to a product for language {Language}.", sku, language);
            }

            var inconsistentItems = basketItems
                .Where(x => !string.IsNullOrWhiteSpace(x.Sku)
                    && productLookup.TryGetValue(x.Sku, out var product)
                    && !HasMatchingProductIdentity(x, product))
                .ToList();

            if (inconsistentItems.Any())
            {
                foreach (var item in inconsistentItems)
                {
                    _logger.LogWarning(
                        "Basket line SKU {Sku} supplied product ID {SubmittedProductId}, but resolved to product ID {ResolvedProductId} for language {Language}.",
                        item.Sku,
                        item.ProductId,
                        productLookup[item.Sku].Id,
                        language);
                }

                throw new CustomException(
                    _orderLocalizer.GetString("BasketPricesCouldNotBeVerified").Value,
                    (int)HttpStatusCode.UnprocessableEntity);
            }

            foreach (var item in basketItems.Where(x => !string.IsNullOrWhiteSpace(x.Sku) && productLookup.ContainsKey(x.Sku)))
            {
                var product = productLookup[item.Sku];
                item.ProductId = product.Id;
                item.Sku = product.Sku;
                item.Name = product.Name;
            }

            var indexedProducts = basketItems
                .Select((item, index) => new { item, index })
                .Where(x => !string.IsNullOrWhiteSpace(x.item.Sku) && productLookup.ContainsKey(x.item.Sku))
                .ToList();

            var priceProducts = indexedProducts.Select(async x =>
            {
                var product = productLookup[x.item.Sku];

                return new PriceProduct
                {
                    PrimarySku = product.PrimaryProductSku,
                    ProductVariantSku = product.Sku,
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
                    BodyColour = await _productColorsService.ToEnglishAsync(_productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleBodyColorAttributeKeys)),
                    ShelfType = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleShelfTypeAttributeKeys),
                    NumberOfMirrors = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleNumberOfMirrorsAttributeKeys),
                    Led = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleLedAttributeKeys).ToYesOrNo(),
                    IsOutlet = GetOutletPriceDriver(x.item)
                };
            });

            var prices = await _priceService.GetPriceResultsForBasketAsync(
                _options.Value.GrulaAccessToken,
                DateTime.UtcNow,
                await Task.WhenAll(priceProducts),
                new PriceClient
                {
                    Id = User.GetClientId(),
                    Name = User.Identity?.Name,
                    CurrencyCode = User.FindFirst(ClaimsEnrichmentConstants.CurrencyClaimType)?.Value,
                    ExtraPacking = User.FindFirst(ClaimsEnrichmentConstants.ExtraPackingClaimType)?.Value,
                    PaletteLoading = User.FindFirst(ClaimsEnrichmentConstants.PaletteLoadingClaimType)?.Value,
                    Country = User.FindFirst(ClaimsEnrichmentConstants.CountryClaimType)?.Value,
                    DeliveryZipCode = User.FindFirst(ClaimsEnrichmentConstants.ZipCodeClaimType)?.Value,
                    DiscountCode = discountCode
                });

            var alignedPrices = AlignPrices(
                basketItems.Count,
                indexedProducts.Select(x => x.index).ToList(),
                prices?.ToList());

            if (alignedPrices is null)
            {
                _logger.LogWarning(
                    "Grula returned {PriceResultCount} price results for {PricedLineCount} priced basket lines; the basket will be persisted unpriced.",
                    prices?.Count,
                    indexedProducts.Count);
            }

            if (!ApplyPrices(basketItems, alignedPrices))
            {
                throw new CustomException(
                    _orderLocalizer.GetString("BasketPricesCouldNotBeVerified").Value,
                    (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        internal static bool HasMatchingProductIdentity(BasketItemRequestModel basketItem, Product product)
        {
            return basketItem.ProductId.HasValue
                && basketItem.ProductId.Value != Guid.Empty
                && basketItem.ProductId.Value == product.Id;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _basketRepository.DeleteAsync(token, language, id);

            Response.Cookies.Delete(BasketConstants.BasketCookieName);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _orderLocalizer.GetString("BasketDeletedSuccessfully").Value });
        }
    }
}
