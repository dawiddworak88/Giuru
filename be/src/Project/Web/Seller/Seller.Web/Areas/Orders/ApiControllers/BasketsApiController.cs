using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Localization;
using Foundation.Media.Services.MediaServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Clients.Repositories.DeliveryAddresses;
using Seller.Web.Areas.Clients.Repositories.FieldValues;
using Seller.Web.Areas.Global.DomainModels;
using Seller.Web.Areas.Global.Repositories;
using Seller.Web.Areas.Orders.ApiRequestModels;
using Seller.Web.Areas.Orders.ApiResponseModels;
using Seller.Web.Areas.Orders.Definitions;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.Repositories.Baskets;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Shared.Repositories.Products;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.Definitions;
using Seller.Web.Shared.DomainModels.Prices;
using Seller.Web.Shared.Repositories.Clients;
using Seller.Web.Shared.Services.Baskets;
using Seller.Web.Shared.Services.Prices;
using Seller.Web.Shared.Services.ProductColors;
using Seller.Web.Shared.Services.Products;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class BasketsApiController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMediaService _mediaService;
        private readonly IProductsRepository _productsRepository;
        private readonly IPriceService _priceService;
        private readonly IProductsService _productsService;
        private readonly IProductColorsService _productColorsService;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IOptions<AppSettings> _options;
        private readonly IClientsRepository _clientsRepository;
        private readonly ICountriesRepository _countriesRepository;
        private readonly IClientFieldValuesRepository _clientFieldValuesRepository;
        private readonly IClientAddressesRepository _clientAddressesRepository;
        private readonly ICurrenciesRepository _currenciesRepository;
        private readonly ILogger<BasketsApiController> _logger;

        public BasketsApiController(
            IBasketRepository basketRepository,
            LinkGenerator linkGenerator,
            IMediaService mediaService,
            IProductsRepository productsRepository,
            IPriceService priceService,
            IProductsService productsService,
            IProductColorsService productColorsService,
            IStringLocalizer<OrderResources> orderLocalizer,
            IOptions<AppSettings> options,
            IClientsRepository clientsRepository,
            ICountriesRepository countriesRepository,
            IClientFieldValuesRepository clientFieldValuesRepository,
            IClientAddressesRepository clientAddressesRepository,
            ICurrenciesRepository currenciesRepository,
            ILogger<BasketsApiController> logger)
        {
            _basketRepository = basketRepository;
            _linkGenerator = linkGenerator;
            _mediaService = mediaService;
            _productsRepository = productsRepository;
            _priceService = priceService;
            _productsService = productsService;
            _productColorsService = productColorsService;
            _orderLocalizer = orderLocalizer;
            _options = options;
            _clientsRepository = clientsRepository;
            _countriesRepository = countriesRepository;
            _clientFieldValuesRepository = clientFieldValuesRepository;
            _clientAddressesRepository = clientAddressesRepository;
            _currenciesRepository = currenciesRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveBasketRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;
            var items = model.Items.OrEmptyIfNull().ToList();
            string discountCode;

            if (_options.Value.IsGrulaConfigured)
            {
                var existingBasket = model.Id.HasValue
                    && DiscountCodeResolver.RequiresPersistedDiscountCode(model.HasDiscountCode, model.DiscountCode, items.Any())
                    ? await _basketRepository.GetBasketByIdAsync(token, language, model.Id)
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
                var existingBasket = model.Id.HasValue
                    ? await _basketRepository.GetBasketByIdAsync(token, language, model.Id)
                    : null;
                discountCode = existingBasket?.DiscountCode;
            }

            if (_options.Value.IsGrulaConfigured && items.Any())
            {
                await RepriceBasketItemsAsync(token, language, model.ClientId, items, discountCode);
            }
            else if (items.Any())
            {
                ClearUntrustedPrices(items);
            }

            var basket = await _basketRepository.SaveAsync(token, language, model.Id,
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
                var canSeePrices = _priceService.CanSeePrices(model.ClientId);

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
            Guid? clientId,
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

            var client = await _clientsRepository.GetClientAsync(token, _options.Value.DefaultCulture, clientId);
            var countries = await _countriesRepository.GetAsync(token, _options.Value.DefaultCulture, $"{nameof(Country.CreatedDate)} desc");
            var clientCountryName = client?.CountryId.HasValue is true
                ? countries.OrEmptyIfNull().FirstOrDefault(x => x.Id == client.CountryId)?.Name
                : null;
            string deliveryZipCode = null;

            if (client?.DefaultDeliveryAddressId.HasValue is true)
            {
                var clientAddress = await _clientAddressesRepository.GetAsync(token, _options.Value.DefaultCulture, client.DefaultDeliveryAddressId);
                var deliveryCountry = countries.OrEmptyIfNull().FirstOrDefault(x => x.Id == clientAddress?.CountryId);

                if (clientAddress is not null && deliveryCountry is not null)
                {
                    deliveryZipCode = $"{clientAddress.PostCode} ({clientAddress.City}, {deliveryCountry.Name})";
                }
            }

            var clientFieldValues = await _clientFieldValuesRepository.GetAsync(token, _options.Value.DefaultCulture, clientId);
            var currency = await _currenciesRepository.GetAsync(token, _options.Value.DefaultCulture, client?.PreferedCurrencyId);
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
                DateTime.UtcNow,
                await Task.WhenAll(priceProducts),
                new PriceClient
                {
                    Id = client?.Id,
                    Name = client?.Name,
                    CurrencyCode = currency?.CurrencyCode,
                    ExtraPacking = clientFieldValues.OrEmptyIfNull().FirstOrDefault(x => x.FieldName == ClaimsEnrichmentConstants.ExtraPackingClientFieldName)?.FieldValue.ToYesOrNo(),
                    PaletteLoading = clientFieldValues.OrEmptyIfNull().FirstOrDefault(x => x.FieldName == ClaimsEnrichmentConstants.PaletteLoadingClientFieldName)?.FieldValue.ToYesOrNo(),
                    Country = clientCountryName,
                    DeliveryZipCode = deliveryZipCode,
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
    }
}
