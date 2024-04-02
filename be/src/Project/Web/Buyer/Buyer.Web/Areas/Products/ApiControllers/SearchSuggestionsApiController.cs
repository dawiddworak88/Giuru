using Buyer.Web.Areas.Products.Services.Products;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class SearchSuggestionsApiController : BaseApiController
    {
        private readonly IProductsService _productsService;

        public SearchSuggestionsApiController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int size, int searchArea)
        {
            var suggestions = await _productsService.GetProductSuggestionsAsync(
                searchTerm,
                size,
                CultureInfo.CurrentUICulture.Name,
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                searchArea);

            return StatusCode((int)HttpStatusCode.OK, suggestions);
        }
    }
}
