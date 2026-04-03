using Buyer.Web.Areas.Products.Repositories;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Areas.Products.Services.DeliveryMessages;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Middlewares;
using Buyer.Web.Shared.DomainModels.Prices;
using Buyer.Web.Shared.Repositories.LeadTime;
using Buyer.Web.Shared.Services.DeliveryDates;
using Buyer.Web.Shared.Services.Prices;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Helpers;
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class AvailableProductsApiController : BaseApiController
    {
        private readonly IProductsService productsService;
        private readonly IInventoryRepository inventoryRepository;
        private readonly IOptions<AppSettings> _options;
        private readonly IPriceService _priceService;
        private readonly IOutletRepository _outletRepository;
        private readonly ILeadTimeRepository _leadTimeRepository;
        private readonly IDeliveryMessageHelper _deliveryMessageHelper;
        private readonly IExpectedDeliveryDateService _expectedDeliveryDateService;

        public AvailableProductsApiController(
            IProductsService productsService,
            IInventoryRepository inventoryRepository,
            IPriceService priceService,
            IOptions<AppSettings> options,
            IOutletRepository outletRepository,
            ILeadTimeRepository leadTimeRepository,
            IDeliveryMessageHelper deliveryMessageHelper,
            IExpectedDeliveryDateService expectedDeliveryDateService)
        {
            this.productsService = productsService;
            this.inventoryRepository = inventoryRepository;
            _options = options;
            _priceService = priceService;
            _leadTimeRepository = leadTimeRepository;
            _outletRepository = outletRepository;
            _deliveryMessageHelper = deliveryMessageHelper;
            _expectedDeliveryDateService = expectedDeliveryDateService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);

            var inventories = await this.inventoryRepository.GetAvailbleProductsInventory(
                CultureInfo.CurrentUICulture.Name,
                pageIndex,
                itemsPerPage,
                token);

            if (inventories?.Data is not null && inventories.Data.Any())
            {
                var products = await this.productsService.GetProductsAsync(
                    inventories.Data.Select(x => x.ProductId),
                    null,
                    null,
                    CultureInfo.CurrentUICulture.Name,
                    null,
                    false,
                    pageIndex,
                    itemsPerPage,
                    await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName));

                var outletItems = await _outletRepository.GetOutletProductsByProductsIdAsync(
                        await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                        CultureInfo.CurrentUICulture.Name,
                        inventories.Data.Select(x => x.ProductId));

                if (products is not null)
                {
                    var prices = Enumerable.Empty<Price>();

                    if (string.IsNullOrWhiteSpace(_options.Value.GrulaAccessToken) is false)
                    {
                        prices = await _priceService.GetPrices(
                            _options.Value.GrulaAccessToken,
                            DateTime.UtcNow,
                            products.Data.Select(x => new PriceProduct
                            {
                                PrimarySku = x.PrimaryProductSku,
                                FabricsGroup = x.FabricsGroup,
                                ExtraPacking = x.ExtraPacking,
                                SleepAreaSize = x.SleepAreaSize,
                                PaletteSize = x.PaletteSize,
                                Size = x.Size,
                                PointsOfLight = x.PointsOfLight,
                                LampshadeType = x.LampshadeType,
                                LampshadeSize = x.LampshadeSize,
                                LinearLight = x.LinearLight,
                                Mirror = x.Mirror,
                                Shape = x.Shape,
                                PrimaryColor = x.PrimaryColor,
                                SecondaryColor = x.SecondaryColor,
                                ShelfType = x.ShelfType
                            }),
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

                    var leadTimes = await _leadTimeRepository.GetLeadTimesAsync(
                        accessToken: token,
                        skus: [.. products.Data.Select(x => x.Sku)]);

                    for (int i = 0; i < products.Data.Count(); i++)
                    {
                        var product = products.Data.ElementAtOrDefault(i);

                        if (product is null)
                        {
                            continue;
                        }

                        var availableQuantity = inventories.Data.FirstOrDefault(x => x.ProductId == product.Id)?.AvailableQuantity;

                        if (availableQuantity > 0)
                        {
                            product.CanOrder = true;
                            product.AvailableQuantity = availableQuantity;
                        }

                        if (prices.Any())
                        {
                            var price = prices.ElementAtOrDefault(i);

                            if (price is not null)
                            {
                                product.Price = new ProductPriceViewModel
                                {
                                    Current = price.CurrentPrice,
                                    Currency = price.CurrencyCode
                                };
                            }
                        }

                        product.InStock = true;
                        product.ExpectedDelivery = inventories.Data.FirstOrDefault(x => x.ProductId == product.Id)?.ExpectedDelivery;
                        
                        var leadTimeDays = leadTimes?.Items?.FirstOrDefault(x => x.Sku == product.Sku)?.LeadTimeDays ?? 0;
                        
                        product.ExpectedLeadTime = leadTimeDays > 0
                            ? _expectedDeliveryDateService.CalculateExpectedDeliveryDate(leadTimeDays)
                            : null;
                        
                        product.LeadTimeDeliveryMessage = _deliveryMessageHelper.GetDeliveryMessage(
                            User.FindFirst(ClaimsEnrichmentConstants.DeliveryTypeClaimType)?.Value, product.InStock, product.ExpectedDelivery);
                    }

                    return this.StatusCode((int)HttpStatusCode.OK, new PagedResults<IEnumerable<CatalogItemViewModel>>(inventories.Total, itemsPerPage) { Data = products.Data.OrderByDescending(x => x.AvailableQuantity) });
                }
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
