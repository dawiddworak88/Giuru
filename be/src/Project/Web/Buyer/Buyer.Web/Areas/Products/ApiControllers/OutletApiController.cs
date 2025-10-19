using Buyer.Web.Areas.Products.Repositories;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Middlewares;
using Buyer.Web.Shared.DomainModels.Prices;
using Buyer.Web.Shared.Services.Prices;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Foundation.Search.Binders;
using Foundation.Search.Models;
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
    public class OutletApiController : BaseApiController
    {
        private readonly IProductsService productsService;
        private readonly IOutletRepository outletRepository;
        private readonly IOptions<AppSettings> _options;
        private readonly IPriceService _priceService;

        public OutletApiController(
            IProductsService productsService,
            IOutletRepository outletRepository,
            IOptions<AppSettings> options,
            IPriceService priceService)
        {
            this.productsService = productsService;
            this.outletRepository= outletRepository;
            _options = options;
            _priceService = priceService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [ModelBinder(BinderType = typeof(SearchQueryFiltersBinder))] QueryFilters filters,
            int pageIndex, 
            int itemsPerPage, 
            string orderBy)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var products = await this.productsService.GetProductsAsync(
                token,
                language,
                null,
                pageIndex,
                itemsPerPage,
                "outlet",
                orderBy,
                filters);

            if (products.Data.OrEmptyIfNull().Any())
            {
                var outletItems = await this.outletRepository.GetOutletProductsByProductsIdAsync(
                    token,
                    language,
                    products.Data.Select(x => x.Id));

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
                            SleepAreaSize = x.SleepAreaSize,
                            ExtraPacking = x.ExtraPacking,
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
                            ShelfType = x.ShelfType,
                            IsOutlet = (outletItems.FirstOrDefault(y => y.ProductId == x.Id)?.AvailableQuantity > 0).ToYesOrNo()
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

                for (int i = 0; i < products.Data.Count(); i++)
                {
                    var product = products.Data.ElementAtOrDefault(i);

                    if (product is null)
                    {
                        continue;
                    }

                    var availableOutletQuantity = outletItems.FirstOrDefault(x => x.ProductId == product.Id)?.AvailableQuantity;

                    if (availableOutletQuantity > 0)
                    {
                        product.AvailableOutletQuantity = availableOutletQuantity;
                        product.CanOrder = true;
                        product.InOutlet = true;
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
                }

                return this.StatusCode((int)HttpStatusCode.OK, new PagedResults<IEnumerable<CatalogItemViewModel>>(products.Total, itemsPerPage)
                {
                    Data = products.Data
                });
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
