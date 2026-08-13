using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
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
using Seller.Web.Areas.Global.Repositories;
using Seller.Web.Areas.Media.ApiRequestModels;
using Seller.Web.Areas.Orders.ApiResponseModels;
using Seller.Web.Areas.Orders.Definitions;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.Repositories.Baskets;
using Seller.Web.Areas.Orders.Repositories.Orders;
using Seller.Web.Areas.Orders.Services.Basket;
using Seller.Web.Areas.Orders.Services.OrderFiles;
using Seller.Web.Areas.Shared.Repositories.Media;
using Seller.Web.Areas.Shared.Repositories.Products;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.Definitions;
using Seller.Web.Shared.DomainModels.Media;
using Seller.Web.Shared.DomainModels.Prices;
using Seller.Web.Shared.Repositories.Clients;
using Seller.Web.Shared.Repositories.Inventory;
using Seller.Web.Shared.Services.Baskets;
using Seller.Web.Shared.Services.Prices;
using Seller.Web.Shared.Services.ProductColors;
using Seller.Web.Shared.Services.Products;
using Seller.Web.Areas.Global.DomainModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class OrderFileApiController : BaseApiController
    {
        private readonly IOrderFileService _orderFileService;
        private readonly IProductsRepository _productsRepository;
        private readonly IBasketRepository _basketRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMediaService _mediaService;
        private readonly ILogger<OrderFileApiController> _logger;
        private readonly IMediaItemsRepository _mediaRepository;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IPriceService _priceService;
        private readonly IProductsService _productsService;
        private readonly IProductColorsService _productColorsService;
        private readonly IOptions<AppSettings> _options;
        private readonly IClientsRepository _clientsRepository;
        private readonly ICountriesRepository _countriesRepository;
        private readonly IClientFieldValuesRepository _clientFieldValuesRepository;
        private readonly IClientAddressesRepository _clientAddressesRepository;
        private readonly ICurrenciesRepository _currenciesRepository;

        public OrderFileApiController(
            IOrderFileService orderFileService,
            IProductsRepository productsRepository,
            IBasketRepository basketRepository,
            LinkGenerator linkGenerator,
            IMediaService mediaService,
            IMediaItemsRepository mediaRepository,
            IOrdersRepository ordersRepository,
            IInventoryRepository inventoryRepository,
            ILogger<OrderFileApiController> logger,
            IStringLocalizer<OrderResources> orderLocalizer,
            IPriceService priceService,
            IProductsService productsService,
            IProductColorsService productColorsService,
            IOptions<AppSettings> options,
            IClientsRepository clientsRepository,
            ICountriesRepository countriesRepository,
            IClientFieldValuesRepository clientFieldValuesRepository,
            IClientAddressesRepository clientAddressesRepository,
            ICurrenciesRepository currenciesRepository)
        {
            _orderFileService = orderFileService;
            _productsRepository = productsRepository;
            _basketRepository = basketRepository;
            _linkGenerator = linkGenerator;
            _mediaService = mediaService;
            _logger = logger;
            _mediaRepository = mediaRepository;
            _ordersRepository = ordersRepository;
            _inventoryRepository = inventoryRepository;
            _orderLocalizer = orderLocalizer;
            _priceService = priceService;
            _productsService = productsService;
            _productColorsService = productColorsService;
            _options = options;
            _clientsRepository = clientsRepository;
            _countriesRepository = countriesRepository;
            _clientFieldValuesRepository = clientFieldValuesRepository;
            _clientAddressesRepository = clientAddressesRepository;
            _currenciesRepository = currenciesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] UploadMediaRequestModel model)
        {
            var importedOrderLines = OrderBasketUploadHelper.GroupImportedLines(_orderFileService.ImportOrderLines(model.File));

            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;
            var existingBasket = model.Id.HasValue
                ? await _basketRepository.GetBasketByIdAsync(token, language, model.Id)
                : null;
            var canSeePrices = _priceService.CanSeePrices(model.ClientId);
            string discountCode;

            if (_options.Value.IsGrulaConfigured)
            {
                var hasDiscountCode = Request.Form.ContainsKey(nameof(UploadMediaRequestModel.DiscountCode));
                discountCode = DiscountCodeResolver.ResolveDiscountCode(hasDiscountCode, model.DiscountCode, existingBasket?.DiscountCode);
            }
            else
            {
                // Do not activate or remove a Grula discount while pricing is unavailable.
                discountCode = existingBasket?.DiscountCode;
            }

            var importedSkus = importedOrderLines.OrEmptyIfNull().Select(x => x.Sku).Distinct().ToList();
            var skus = importedSkus
                .Concat(existingBasket?.Items.OrEmptyIfNull().Select(x => x.ProductSku) ?? Enumerable.Empty<string>())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct();
            var products = await _productsRepository.GetProductsBySkusAsync(token, language, skus);

            if (products.OrEmptyIfNull().Any(x => importedSkus.Contains(x.Sku)) is false)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new { Message = _orderLocalizer.GetString("ProductsNotFound").Value });
            }

            var productLookup = OrderBasketUploadHelper.CreateProductLookup(products);

            var productIds = products.OrEmptyIfNull().Select(x => x.Id).Distinct().ToList();
            var stockAvailableProducts = await _inventoryRepository.GetAvailbleProductsByProductIdsAsync(token, language, productIds);

            var stockByProductId = stockAvailableProducts
               .OrEmptyIfNull()
               .ToDictionary(g => g.ProductId, g => (double)g.AvailableQuantity);

            OrderBasketUploadHelper.DeductExistingStock(stockByProductId, existingBasket?.Items);

            var basketItems = new List<BasketItem>();

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

            if (!_options.Value.IsGrulaConfigured)
            {
                BasketPriceApplier.ClearUntrustedPrices(completeBasketItems);
            }
            else
            {
                await RepriceMergedBasketAsync(token, language, model.ClientId, completeBasketItems, productLookup, discountCode);
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

            var basket = await _basketRepository.SaveAsync(
                token,
                language,
                model.Id,
                completeBasketItems,
                discountCode);

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
                    ProductUrl = _linkGenerator.GetPathByAction("Edit", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.ProductId }),
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
            Guid? clientId,
            IList<BasketItem> basketItems,
            IReadOnlyDictionary<string, Product> productLookup,
            string discountCode)
        {
            var indexedProducts = basketItems
                .Select((item, index) => new { item, index })
                .Where(x => !string.IsNullOrWhiteSpace(x.item.ProductSku) && productLookup.ContainsKey(x.item.ProductSku))
                .ToList();

            foreach (var item in basketItems.Where(x => string.IsNullOrWhiteSpace(x.ProductSku) || !productLookup.ContainsKey(x.ProductSku)))
            {
                _logger.LogWarning("Basket line SKU {Sku} could not be resolved to a product for language {Language}.", item.ProductSku, language);
            }

            var client = await _clientsRepository.GetClientAsync(token, _options.Value.DefaultCulture, clientId);
            var countries = await _countriesRepository.GetAsync(token, _options.Value.DefaultCulture, $"{nameof(Country.CreatedDate)} desc");
            var clientCountryName = OrderFilePricingContext.GetClientCountryName(client, countries);
            var defaultDeliveryAddressId = OrderFilePricingContext.GetDefaultDeliveryAddressId(client);
            var clientAddress = defaultDeliveryAddressId.HasValue
                ? await _clientAddressesRepository.GetAsync(token, _options.Value.DefaultCulture, defaultDeliveryAddressId)
                : null;
            var deliveryZipCode = OrderFilePricingContext.GetDeliveryZipCode(clientAddress, countries);
            var clientFieldValues = await _clientFieldValuesRepository.GetAsync(token, _options.Value.DefaultCulture, clientId);
            var currency = await _currenciesRepository.GetAsync(token, _options.Value.DefaultCulture, client?.PreferedCurrencyId);

            var priceProducts = indexedProducts.Select(async x =>
            {
                var product = productLookup[x.item.ProductSku];
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
                    IsOutlet = (x.item.OutletQuantity > 0).ToYesOrNo()
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

            var alignedPrices = BasketPriceApplier.AlignPrices(basketItems.Count, indexedProducts.Select(x => x.index).ToList(), prices?.ToList());
            if (alignedPrices is null)
            {
                _logger.LogWarning("Grula returned {PriceResultCount} price results for {PricedLineCount} priced basket lines; the basket will be persisted unpriced.", prices?.Count, indexedProducts.Count);
            }

            if (!BasketPriceApplier.ApplyPrices(basketItems, alignedPrices))
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
                var files = await _mediaRepository.GetMediaItemsAsync(filesIds, language, FilesConstants.DefaultPageIndex, FilesConstants.DefaultPageSize, token);

                foreach (var file in files.Data.OrEmptyIfNull())
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