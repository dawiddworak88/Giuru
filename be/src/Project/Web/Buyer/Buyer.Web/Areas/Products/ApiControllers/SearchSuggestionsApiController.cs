using Buyer.Web.Areas.Products.Services.Inventories;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.Services.SearchSuggestions;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Shared.Definitions.Header;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class SearchSuggestionsApiController : BaseApiController
    {
        private readonly ISearchSuggestionsService _searchSuggestionsService;

        public SearchSuggestionsApiController(ISearchSuggestionsService searchSuggestionsService)
        {
            _searchSuggestionsService = searchSuggestionsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int size, string searchArea)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);

            var suggestions = await _searchSuggestionsService.GetSuggestionsAsync(
                token,
                CultureInfo.CurrentUICulture.Name,
                searchTerm,
                size,
                searchArea);

            return StatusCode((int)HttpStatusCode.OK, suggestions);
        }
    }
}
