using Buyer.Web.Areas.Products.Services.SearchSuggestions.BaseSearchSuggestions;
using Buyer.Web.Areas.Products.ViewModels.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.SearchSuggestions
{
    public class SearchSuggestionsService : ISearchSuggestionsService
    {
        private IBaseSearchSuggestionsService _searchSuggestionsService;

        public async Task<IEnumerable<ProductSuggestionViewModel>> GetSuggestionsAsync(string token, string language, string searchTerm, int size)
        {
            return await _searchSuggestionsService.GetSuggestionsAsync(token, language, searchTerm, size);
        }

        public void SetSearchingArea(IBaseSearchSuggestionsService searchSuggestionsService)
        {
            _searchSuggestionsService = searchSuggestionsService;
        }
    }
}