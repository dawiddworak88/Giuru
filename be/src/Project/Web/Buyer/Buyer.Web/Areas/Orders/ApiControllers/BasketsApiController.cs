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
using Buyer.Web.Shared.Extensions;
using Foundation.Pricing.Baskets;
using Foundation.Pricing.Services;
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

        private readonly IPriceProductFactory _priceProductFactory;
        private readonly IPriceClientResolver _priceClientResolver;
        private readonly IBasketRepricingService _basketRepricingService;

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
            ILogger<BasketsApiController> logger,
            IPriceProductFactory priceProductFactory,
            IPriceClientResolver priceClientResolver,
            IBasketRepricingService basketRepricingService)
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
            _priceProductFactory = priceProductFactory;
            _priceClientResolver = priceClientResolver;
            _basketRepricingService = basketRepricingService;
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

            var discountOutcome = await BasketDiscountCodeCoordinator.ResolveAsync(
                _options.Value.IsGrulaConfigured,
                model.HasDiscountCode,
                model.DiscountCode,
                items.Any(),
                async () => (await _basketRepository.GetBasketById(token, language, id))?.DiscountCode,
                () => _orderLocalizer.GetString("DiscountCodeRequiresBasketItems").Value);

            if (discountOutcome.IsRejected)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new { Message = discountOutcome.RejectionMessage });
            }

            var discountCode = discountOutcome.DiscountCode;

            if (_options.Value.IsGrulaConfigured && items.Any())
            {
                await RepriceBasketItemsAsync(token, language, items, discountCode);
            }
            else if (items.Any())
            {
                BasketPriceApplier.ClearUntrustedPrices(items);
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

            var priceClient = await _priceClientResolver.ResolveAsync(null, discountCode, token);

            var outcome = await _basketRepricingService.RepriceAsync(
                basketItems,
                x => x.Sku,
                x => x.OutletQuantity > 0,
                productLookup,
                (product, isOutletPurchase) => _priceProductFactory.CreateAsync(product, isOutletPurchase),
                priceClient,
                DateTime.UtcNow);

            if (!outcome.Succeeded)
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
