using Buyer.Web.Areas.Products.Repositories;
using Buyer.Web.Areas.Products.Services.DeliveryMessages;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Middlewares;
using Foundation.Pricing.DomainModels;
using Foundation.Pricing.Services;
using Buyer.Web.Shared.Repositories.LeadTime;
using Buyer.Web.Shared.Services.DeliveryDates;
using Buyer.Web.Shared.Services.Prices;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
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
    public class OutletApiController : BaseApiController
    {
        private readonly IProductsService productsService;
        private readonly IOutletRepository outletRepository;
        private readonly IOptions<AppSettings> _options;
        private readonly IPriceService _priceService;
        private readonly ILeadTimeRepository _leadTimeRepository;
        private readonly IDeliveryMessageHelper _deliveryMessageHelper;
        private readonly IExpectedDeliveryDateService _expectedDeliveryDateService;
        private readonly IPriceProductFactory _priceProductFactory;
        private readonly IPriceClientResolver _priceClientResolver;

        public OutletApiController(
            IProductsService productsService,
            IOutletRepository outletRepository,
            IOptions<AppSettings> options,
            IPriceService priceService,
            ILeadTimeRepository leadTimeRepository,
            IDeliveryMessageHelper deliveryMessageHelper,
            IExpectedDeliveryDateService expectedDeliveryDateService,
            IPriceProductFactory priceProductFactory,
            IPriceClientResolver priceClientResolver)
        {
            this.productsService = productsService;
            this.outletRepository= outletRepository;
            _options = options;
            _priceService = priceService;
            _leadTimeRepository = leadTimeRepository;
            _deliveryMessageHelper = deliveryMessageHelper;
            _expectedDeliveryDateService = expectedDeliveryDateService;
            _priceProductFactory = priceProductFactory;
            _priceClientResolver = priceClientResolver;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int pageIndex, int itemsPerPage, string discountCode = null)
        {
            var language = CultureInfo.CurrentUICulture.Name;
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);

            var outletItems = await this.outletRepository.GetOutletProductsAsync(language, pageIndex, itemsPerPage, token);

            if (outletItems?.Data is not null && outletItems.Data.Any())
            {
                var products = await this.productsService.GetProductsAsync(
                    outletItems.Data.Select(x => x.ProductId), null, null, language, null, false, pageIndex, itemsPerPage, token);

                if (products is not null)
                {
                    var prices = Enumerable.Empty<Price>();

                    if (_options.Value.IsGrulaConfigured)
                    {
                        var priceClient = await _priceClientResolver.ResolveAsync(null, discountCode, token);

                        prices = await _priceService.GetPrices(
                            DateTime.UtcNow,
                            products.Data.Select(x => _priceProductFactory.Create(x, isOutletPurchase: true)),
                            priceClient);
                    }

                    var leadTimes = await _leadTimeRepository.GetLeadTimesAsync(
                        accessToken: token,
                        skus: [..products.Data.Select(x => x.Sku)]);

                    for (int i = 0; i < products.Data.Count(); i++)
                    {
                        var product = products.Data.ElementAtOrDefault(i);

                        if (product is null)
                        {
                            continue;
                        }

                        var availableQuantity = outletItems.Data.FirstOrDefault(x => x.ProductId == product.Id)?.AvailableQuantity;

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

                        product.InOutlet = true;
                        product.ExpectedDelivery = outletItems.Data.FirstOrDefault(x => x.ProductId == product.Id)?.ExpectedDelivery;
                        
                        var leadTimeDays = leadTimes?.FirstOrDefault(x => x.Sku == product.Sku)?.LeadTimeDays ?? 0;
                        
                        product.ExpectedLeadTime = leadTimeDays > 0
                            ? _expectedDeliveryDateService.CalculateExpectedDeliveryDate(leadTimeDays)
                            : null;
                        
                        product.LeadTimeDeliveryMessage = _deliveryMessageHelper.GetDeliveryMessage(
                            User.FindFirst(ClaimsEnrichmentConstants.DeliveryTypeClaimType)?.Value, product.InStock, product.ExpectedDelivery);
                    }

                    return this.StatusCode((int)HttpStatusCode.OK, new PagedResults<IEnumerable<CatalogItemViewModel>>(outletItems.Total, itemsPerPage) { Data = products.Data.OrderByDescending(x => x.AvailableQuantity) });
                }
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
