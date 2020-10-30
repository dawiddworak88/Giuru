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
        private readonly IProductsService productsService;

        public SearchSuggestionsApiController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int size)
        {
            var suggestions = await this.productsService.GetProductSuggestionsAsync(
                searchTerm,
                size,
                CultureInfo.CurrentUICulture.Name,
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName));

            return this.StatusCode((int)HttpStatusCode.OK, suggestions);
        }
    }
}
