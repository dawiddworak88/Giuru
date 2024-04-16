using Buyer.Web.Areas.Products.Services.Inventory;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Shared.Definitions.Header;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class SearchSuggestionsApiController : BaseApiController
    {
        private readonly IProductsService _productsService;
        private readonly IInventoryService _inventoryService;

        public SearchSuggestionsApiController(
            IProductsService productsService,
            IInventoryService inventoryService)
        {
            _productsService = productsService;
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int size, string searchArea)
        {
            IEnumerable<string> suggestions;

            if (searchArea == SearchConstants.SearchArea.StockLevel)
            {
                suggestions = await _inventoryService.GetInventoryProductSuggestionsAsync(
                    searchTerm,
                    size,
                    CultureInfo.CurrentUICulture.Name,
                    await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                    searchArea);
            }
            else
            {
                suggestions = await _productsService.GetProductSuggestionsAsync(
                    searchTerm,
                    size,
                    CultureInfo.CurrentUICulture.Name,
                    await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                    searchArea);
            }
            

            return StatusCode((int)HttpStatusCode.OK, suggestions);
        }
    }
}
