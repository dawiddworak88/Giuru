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
using Foundation.Pricing.Services;
using Seller.Web.Shared.Services.Clients;
using Seller.Web.Shared.Services.Prices;
using Seller.Web.Shared.Services.Products;
using System.Collections.Generic;
using Seller.Web.Shared.Services.ProductColors;
using Seller.Web.Shared.Repositories.LeadTime;
using Seller.Web.Shared.Services.DeliveryDates;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Products")]
    public class ProductsApiController : BaseApiController
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IStringLocalizer _productLocalizer;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IOutletRepository _outletRepository;
        private readonly IClientLookupService _clientLookupService;
        private readonly IPriceClientResolver _priceClientResolver;
        private readonly IProductsService _productsService;
        private readonly IProductColorsService _productColorsService;
        private readonly ILeadTimeRepository _leadTimeRepository;
        private readonly IExpectedDeliveryDateService _expectedDeliveryDateService;
        private readonly IPriceProductFactory _priceProductFactory;
        private readonly IProductPricingService _productPricingService;

        public ProductsApiController(
            IProductsRepository productsRepository,
            IStringLocalizer<ProductResources> productLocalizer,
            IInventoryRepository inventoryRepository,
            IOutletRepository outletRepository,
            IClientLookupService clientLookupService,
            IPriceClientResolver priceClientResolver,
            IProductsService productsService,
            IProductColorsService productColorsService,
            ILeadTimeRepository leadTimeRepository,
            IExpectedDeliveryDateService expectedDeliveryDateService,
            IPriceProductFactory priceProductFactory,
            IProductPricingService productPricingService)
        {
            _productsRepository = productsRepository;
            _productLocalizer = productLocalizer;
            _inventoryRepository = inventoryRepository;
            _outletRepository = outletRepository;
            _clientLookupService = clientLookupService;
            _priceClientResolver = priceClientResolver;
            _productsService = productsService;
            _productColorsService = productColorsService;
            _leadTimeRepository = leadTimeRepository;
            _expectedDeliveryDateService = expectedDeliveryDateService;
            _priceProductFactory = priceProductFactory;
            _productPricingService = productPricingService;
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
            int itemsPerPage,
            string discountCode = null)
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

                var client = await _clientLookupService.GetAsync(token, clientId);

                var prices = clientId.HasValue
                    ? await _productPricingService.GetPricesAsync(
                        () => _priceProductFactory.CreateAsync(products.Data, isOutletPurchase: false),
                        () => _priceClientResolver.ResolveAsync(clientId, discountCode, token))
                    : PricedProducts.Empty;

                var leadTimes = await _leadTimeRepository.GetLeadTimesAsync(
                    accessToken: token,
                    customerId: client.OrganisationId,
                    skus: [.. products.Data.Select(x => x.Sku)]);

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

                    var leadTimeDays = leadTimes?.FirstOrDefault(x => x.Sku == product.Sku)?.LeadTimeDays ?? 0;
                    productQuantity.ExpectedLeadTime = leadTimeDays > 0
                        ? DateOnly.FromDateTime(_expectedDeliveryDateService.CalculateExpectedDeliveryDate(leadTimeDays))
                        : null;

                    var price = prices.ElementAtOrDefault(i);

                    if (price is not null)
                    {
                        productQuantity.Price = price.CurrentPrice;
                        productQuantity.Currency = price.CurrencyCode;
                    }

                    productsQuantities.Add(productQuantity);
                }

                return StatusCode((int)HttpStatusCode.OK, productsQuantities);
            }

            return StatusCode((int)HttpStatusCode.OK, products.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetPrice(Guid? clientId, string sku, string discountCode = null, bool isOutlet = false)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var product = await _productsRepository.GetProductAsync(token, language, sku);

            if (clientId.HasValue)
            {
                try
                {
                    var price = await _productPricingService.GetPriceAsync(
                        () => _priceProductFactory.CreateAsync(product, isOutletPurchase: isOutlet),
                        () => _priceClientResolver.ResolveAsync(clientId, discountCode, token));

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
