using Buyer.Web.Areas.Products.Services.SearchSuggestions;
using Buyer.Web.Areas.Products.Services.SearchSuggestions.ProductsSearchSuggestions;
using Buyer.Web.Areas.Products.Services.SearchSuggestions.StockLevelsSearchSuggetions;
using Buyer.Web.Shared.Definitions.Header;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class SearchSuggestionsApiController : BaseApiController
    {
        private readonly ISearchSuggestionsService _searchSuggestionsService;
        private readonly IServiceProvider _serviceProvider;

        public SearchSuggestionsApiController(
            ISearchSuggestionsService searchSuggestionsService,
            IServiceProvider serviceProvider)
        {
            _searchSuggestionsService = searchSuggestionsService;
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int size, string searchArea)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);

            if (searchArea == SearchConstants.SearchArea.StockLevel)
            {
                var area = _serviceProvider.GetRequiredService<StockLevelsSearchSuggestionsService>();

                _searchSuggestionsService.SetSearchingArea(area);
            }
            else
            {
                var area = _serviceProvider.GetRequiredService<ProductsSearchSuggestionsService>();

                _searchSuggestionsService.SetSearchingArea(area);
            }

            var suggestions = await _searchSuggestionsService.GetSuggestionsAsync(
                token,
                CultureInfo.CurrentUICulture.Name,
                searchTerm,
                size);

            return StatusCode((int)HttpStatusCode.OK, suggestions);
        }
    }
}
