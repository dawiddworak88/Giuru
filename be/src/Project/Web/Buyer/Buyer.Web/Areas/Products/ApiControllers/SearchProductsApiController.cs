﻿using Buyer.Web.Areas.Products.Repositories;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.DomainModels.Prices;
using Buyer.Web.Shared.Services.Prices;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class SearchProductsApiController : BaseApiController
    {
        private readonly IProductsService _productsService;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IOutletRepository _outletRepository;
        private readonly IOptions<AppSettings> _options;
        private readonly IPriceService _priceService;

        public SearchProductsApiController(
            IProductsService productsService,
            IOutletRepository outletRepository,
            IInventoryRepository inventoryRepository,
            IOptions<AppSettings> options,
            IPriceService priceService)
        {
            _productsService = productsService;
            _inventoryRepository = inventoryRepository;
            _outletRepository = outletRepository;
            _options = options;
            _priceService = priceService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var products = await _productsService.GetProductsAsync(null, null, null, language, searchTerm, true, pageIndex, itemsPerPage, token);
            
            if (products.Data is not null)
            {
                var outletItems = await _outletRepository.GetOutletProductsByIdsAsync(token, language, products.Data.Select(x => x.Id));
                var inventoryItems = await _inventoryRepository.GetAvailbleProductsInventoryByIds(token, language, products.Data.Select(x => x.Id));

                var prices = Enumerable.Empty<Price>();

                if (string.IsNullOrWhiteSpace(_options.Value.GrulaAccessToken) is false)
                {
                    prices = await _priceService.GetPrices(
                        _options.Value.GrulaAccessToken,
                        "PLN",
                        DateTime.UtcNow,
                        products.Data.Select(x => new PriceProduct
                        {
                            PrimarySku = x.PrimaryProductSku,
                            FabricsGroup = x.FabricsGroup
                        }));
                }

                Console.WriteLine("Prices: " + prices.Count() + " > " + JsonConvert.SerializeObject(prices));
                Console.WriteLine("Products: " + products.Data.Count() + " > " + JsonConvert.SerializeObject(products.Data));

                for (int i = 0; i < products.Data.Count(); i++)
                {
                    var product = products.Data.ElementAtOrDefault(i);

                    if (product is null)
                    {
                        continue;
                    }

                    var outletItem = outletItems.FirstOrDefault(x => x.ProductSku == product.Sku);

                    if (outletItem is not null)
                    {
                        product.InOutlet = true;
                        product.AvailableOutletQuantity = outletItem.AvailableQuantity;
                        product.OutletTitle = outletItem.Title;
                    }

                    var inventoryItem = inventoryItems.FirstOrDefault(x => x.ProductSku == product.Sku);

                    if (inventoryItem is not null)
                    {
                        product.InStock = true;
                        product.AvailableQuantity = inventoryItem.AvailableQuantity;
                        product.ExpectedDelivery = inventoryItem.ExpectedDelivery;
                    }

                    if (prices.Any())
                    {
                        var price = prices.ElementAtOrDefault(i);

                        if (price is not null)
                        {
                            product.Price = new ProductPriceViewModel
                            {
                                Current = price.Amount,
                                Currency = price.CurrencyCode
                            };
                        }
                    }

                    product.CanOrder = true;
                }

                var response = new PagedResults<IEnumerable<CatalogItemViewModel>>(products.Total, products.PageSize)
                {
                    Data = products.Data
                };

                return StatusCode((int)HttpStatusCode.OK, response);
            }

            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}
