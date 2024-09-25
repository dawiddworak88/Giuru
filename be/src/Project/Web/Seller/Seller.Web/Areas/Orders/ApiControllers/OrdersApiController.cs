using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Inventory.Repositories.Inventories;
using Seller.Web.Areas.Orders.ApiResponseModels;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.Repositories.Orders;
using Seller.Web.Areas.Shared.Repositories.Products;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ApiControllers
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

            var orders = await _ordersRepository.GetOrdersAsync(
                token,
                language,
                searchTerm,
                pageIndex,
                itemsPerPage,
                $"{nameof(Order.CreatedDate)} desc");

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
                GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value),
                pageIndex,
                itemsPerPage,
                null);

            var onStockProducts = await _inventoryRepository.GetInventoryProductsAsync(
                token,
                language,
                searchTerm,
                pageIndex,
                itemsPerPage,
                null);

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

                var onStockProduct = onStockProducts?.Data.FirstOrDefault(x => x.ProductId == product.Id);

                if (onStockProduct is not null)
                {
                    suggestion.StockQuantity = onStockProduct.AvailableQuantity;
                }

                suggestions.Add(suggestion);
            }

            return StatusCode((int)HttpStatusCode.OK, suggestions);
        }
    }
}