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
            ICurrenciesRepository currenciesRepository)
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
                    UnitPrice = _priceService.CanSeePrices(model.ClientId) ? x.UnitPrice : null,
                    Price = _priceService.CanSeePrices(model.ClientId) ? x.Price : null,
                    Currency = _priceService.CanSeePrices(model.ClientId) ? x.Currency : null,
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
            if (basketItems is null)
            {
                return;
            }

            // Browser-submitted monetary values are untrusted. Without Grula there is no
            // authoritative price source, so persist the basket explicitly unpriced.
            foreach (var basketItem in basketItems)
            {
                basketItem.UnitPrice = null;
                basketItem.Price = null;
                basketItem.Currency = null;
            }
        }

        internal static bool ApplyPrices(
            IList<BasketItemRequestModel> basketItems,
            IList<PriceLookupResult> priceResults)
        {
            if (basketItems is null)
            {
                return true;
            }

            // A missing or incomplete response cannot be aligned safely, so do not use any
            // browser values (or partially returned values) as a fallback.
            if (priceResults is null || priceResults.Count != basketItems.Count)
            {
                ClearUntrustedPrices(basketItems);
                return true;
            }

            // Validate every explicit status before changing a line. This preserves the
            // all-or-nothing behaviour for invalid price drivers and future unknown states.
            if (priceResults.Any(x => x is not null &&
                x.Status != PriceLookupStatus.Priced &&
                x.Status != PriceLookupStatus.AuthoritativeNoPrice &&
                x.Status != PriceLookupStatus.ServiceUnavailable &&
                x.Status != PriceLookupStatus.MissingResponse))
            {
                return false;
            }

            for (var index = 0; index < basketItems.Count; index++)
            {
                var basketItem = basketItems[index];
                var priceResult = priceResults[index];

                if (priceResult is null ||
                    priceResult.Status == PriceLookupStatus.AuthoritativeNoPrice ||
                    priceResult.Status == PriceLookupStatus.ServiceUnavailable ||
                    priceResult.Status == PriceLookupStatus.MissingResponse ||
                    priceResult.Price is null ||
                    string.IsNullOrWhiteSpace(priceResult.Price.CurrencyCode))
                {
                    basketItem.UnitPrice = null;
                    basketItem.Price = null;
                    basketItem.Currency = null;
                    continue;
                }

                var totalQuantity = basketItem.Quantity + basketItem.StockQuantity + basketItem.OutletQuantity;
                basketItem.UnitPrice = priceResult.Price.CurrentPrice;
                basketItem.Price = priceResult.Price.CurrentPrice * (decimal)totalQuantity;
                basketItem.Currency = priceResult.Price.CurrencyCode;
            }

            return true;
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
            if (unresolvedSkus.Any())
            {
                throw new CustomException("Basket contains products that could not be resolved: " + string.Join(", ", unresolvedSkus.Select(x => x ?? "(blank)")), (int)HttpStatusCode.UnprocessableEntity);
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

            var alignedPrices = new PriceLookupResult[basketItems.Count];

            for (var i = 0; i < indexedProducts.Count && prices is not null && i < prices.Count; i++)
            {
                alignedPrices[indexedProducts[i].index] = prices[i];
            }

            if (!ApplyPrices(basketItems, alignedPrices))
            {
                throw new CustomException("Basket prices contain invalid price drivers.", (int)HttpStatusCode.UnprocessableEntity);
            }
        }
    }
}
