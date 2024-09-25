using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;

namespace Buyer.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class OrdersApiController : BaseApiController
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public OrdersApiController(
            IOrdersRepository ordersRepository,
            IProductsRepository productsRepository,
            IInventoryRepository inventoryRepository)
        {
            _ordersRepository = ordersRepository;
            _productsRepository = productsRepository;
            _inventoryRepository = inventoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var orders = await _ordersRepository.GetOrdersAsync(token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(Order.CreatedDate)} desc");

            return StatusCode((int)HttpStatusCode.OK, orders);
        }

        public async Task<ActionResult> GetSuggestions(
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
                null,
                pageIndex,
                itemsPerPage,
                null);

            var onStockProducts = await _inventoryRepository.GetAvailbleProductsInventoryByIds(
                token,
                language,
                products?.Data?.Select(x => x.Id));

            var suggestions = new List<OrderSuggestionResponseModel>();

            foreach (var product in products.Data.OrEmptyIfNull())
            {
                var suggestion = new OrderSuggestionResponseModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Sku = product.Sku,
                    Images = product.Images,
                };

                var onStockProduct = onStockProducts.FirstOrDefault(x => x.ProductId == product.Id);

                if (onStockProduct is not null)
                {
                    suggestion.StockQuantity = onStockProduct.AvailableQuantity ?? 0;
                }

                suggestions.Add(suggestion);
            }

            return StatusCode((int)HttpStatusCode.OK, suggestions);
        }
    }
}
