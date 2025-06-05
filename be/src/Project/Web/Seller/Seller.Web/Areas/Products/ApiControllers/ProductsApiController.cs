using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.ApiRequestModels;
using System;
using System.Net;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Security.Claims;
using Foundation.Account.Definitions;
using Foundation.Extensions.Helpers;
using Seller.Web.Areas.Shared.Repositories.Products;
using Seller.Web.Areas.Inventory.Repositories.Inventories;
using Foundation.Extensions.ExtensionMethods;
using Seller.Web.Areas.Inventory.Repositories;
using Seller.Web.Areas.Products.ApiResponseModels;
using Seller.Web.Shared.DomainModels.Prices;
using Microsoft.Extensions.Options;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.Repositories.Clients;
using Seller.Web.Areas.Global.Repositories;
using Seller.Web.Areas.Clients.Repositories.FieldValues;
using Seller.Web.Areas.Global.DomainModels;
using Seller.Web.Areas.Clients.Repositories.DeliveryAddresses;
using System.Text.RegularExpressions;
using Seller.Web.Shared.Services.Prices;
using Seller.Web.Shared.Services.Products;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Products")]
    public class ProductsApiController : BaseApiController
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IStringLocalizer _productLocalizer;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IOutletRepository _outletRepository;
        private readonly IPriceService _priceService;
        private readonly IClientsRepository _clientsRepository;
        private readonly ICountriesRepository _countriesRepository;
        private readonly IClientFieldValuesRepository _clientFieldValuesRepository;
        private readonly IClientAddressesRepository _clientAddressesRepository;
        private readonly ICurrenciesRepository _currenciesRepository;
        private readonly IProductsService _productsService;
        private readonly IOptions<AppSettings> _options;

        public ProductsApiController(
            IProductsRepository productsRepository,
            IStringLocalizer<ProductResources> productLocalizer,
            IInventoryRepository inventoryRepository,
            IOutletRepository outletRepository,
            IPriceService priceService,
            IClientsRepository clientsRepository,
            ICountriesRepository countriesRepository,
            IClientFieldValuesRepository clientFieldValuesRepository,
            IClientAddressesRepository clientAddressesRepository,
            ICurrenciesRepository currenciesRepository,
            IProductsService productsService,
            IOptions<AppSettings> options)
        {
            _productsRepository = productsRepository;
            _productLocalizer = productLocalizer;
            _inventoryRepository = inventoryRepository;
            _outletRepository = outletRepository;
            _priceService = priceService;
            _clientsRepository = clientsRepository;
            _options = options;
            _countriesRepository = countriesRepository;
            _clientFieldValuesRepository = clientFieldValuesRepository;
            _clientAddressesRepository = clientAddressesRepository;
            _currenciesRepository = currenciesRepository;
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            string searchTerm,
            bool? hasPrimaryProduct,
            int pageIndex,
            int itemsPerPage)
        {
            var products = await _productsRepository.GetProductsAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                searchTerm,
                hasPrimaryProduct,
                GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value),
                pageIndex,
                itemsPerPage,
                null);

            foreach (var product in products.Data)
            {
                product.Name = $"{product.Name} ({product.Sku})";
            }

            return StatusCode((int)HttpStatusCode.OK, products);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveProductRequestModel model)
        {
            var productId = await _productsRepository.SaveAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                model.Id,
                model.Name,
                model.Sku,
                model.Description,
                model.IsNew,
                model.IsPublished,
                model.PrimaryProductId,
                model.CategoryId,
                model.Images.OrEmptyIfNull().Select(x => x.Id),
                model.Files.OrEmptyIfNull().Select(x => x.Id),
                model.Ean,
                model.FulfillmentTime,
                model.FormData);

            return StatusCode((int)HttpStatusCode.OK, new { Id = productId, Message = _productLocalizer.GetString("ProductSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            await _productsRepository.DeleteAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                id);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _productLocalizer.GetString("ProductDeletedSuccessfully").Value });
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsQuantities(
            Guid? clientId,
            string searchTerm, 
            bool? hasPrimaryProduct, 
            int pageIndex, 
            int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var products = await _productsRepository.GetProductsAsync(
                token,
                language,
                searchTerm,
                hasPrimaryProduct,
                GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value),
                pageIndex,
                itemsPerPage,
                null);

            if (products.Data.Any())
            {
                var inventories = await _inventoryRepository.GetInventoryProductByProductIdsAsync(
                    token,
                    language,
                    products.Data.Select(x => x.Id));

                var outlets = await _outletRepository.GetOutletProductsByProductsIdAsync(
                    token,
                    language,
                    products.Data.Select(x => x.Id));

                var prices = Enumerable.Empty<Price>();

                if (string.IsNullOrWhiteSpace(_options.Value.GrulaAccessToken) is false)
                {
                    var client = await _clientsRepository.GetClientAsync(token, _options.Value.DefaultCulture, clientId);

                    var countries = await _countriesRepository.GetAsync(token, _options.Value.DefaultCulture, $"{nameof(Country.CreatedDate)} desc");

                    var clientCountryName = countries.FirstOrDefault(c => c.Id == client?.CountryId)?.Name;

                    string deliveryZipCode = null;

                    if (client.DefaultDeliveryAddressId.HasValue)
                    {
                        var clientAddress = await _clientAddressesRepository.GetAsync(token, _options.Value.DefaultCulture, clientId);

                        if (clientAddress is not null)
                        {
                            var deliveryCountry = countries.FirstOrDefault(c => c.Id == clientAddress.CountryId);

                            if (deliveryCountry is not null)
                            {
                                deliveryZipCode = $"{clientAddress.PostCode} ({clientAddress.City}, {deliveryCountry.Name})";
                            }
                        }
                    }

                    var clientFieldValues = await _clientFieldValuesRepository.GetAsync(token, _options.Value.DefaultCulture, clientId);

                    var currency = await _currenciesRepository.GetAsync(token, _options.Value.DefaultCulture, client?.PreferedCurrencyId);

                    prices = await _priceService.GetPrices(
                        _options.Value.GrulaAccessToken,
                        DateTime.UtcNow,
                        products.Data.Select(x => new PriceProduct
                        {
                            PrimarySku = x.PrimaryProductSku,
                            FabricsGroup = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossiblePriceGroupAttributeKeys),
                            ExtraPacking = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleExtraPackingAttributeKeys),
                            SleepAreaSize = _productsService.GetSleepAreaSize(x.ProductAttributes),
                            PaletteSize = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossiblePaletteSizeAttributeKeys)
                        }),
                        new PriceClient
                        {
                            Name = client?.Name,
                            CurrencyCode = currency?.CurrencyCode,
                            ExtraPacking = clientFieldValues.FirstOrDefault(x => x.FieldName == "Extra Packing")?.FieldValue,
                            PaletteLoading = clientFieldValues.FirstOrDefault(x => x.FieldName == "Palette Loading")?.FieldValue,
                            Country = clientCountryName,
                            DeliveryZipCode = deliveryZipCode
                        });
                }

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

            return StatusCode((int)HttpStatusCode.OK, products.Data);
        }
    }
}
