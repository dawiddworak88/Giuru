using Buyer.Web.Areas.Products.Services.SearchSuggestions.ProductsSearchSuggestions;
using Buyer.Web.Areas.Products.Services.SearchSuggestions.StockLevelsSearchSuggetions;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Shared.Definitions.Header;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.SearchSuggestions
{
    public class SearchSuggestionsService : ISearchSuggestionsService
    {
        private readonly IProductsSearchSuggestionsService _productsSearchSugestionsService;
        private readonly IStockLevelsSearchSuggestionsService _stockLevelsSearchSuggestionsService;

        public SearchSuggestionsService(
            IProductsSearchSuggestionsService productsSearchSugestionsService,
            IStockLevelsSearchSuggestionsService stockLevelsSearchSuggestionsService)
        {
            _productsSearchSugestionsService = productsSearchSugestionsService;
            _stockLevelsSearchSuggestionsService = stockLevelsSearchSuggestionsService;
        }

        public async Task<IEnumerable<ProductSuggestionViewModel>> GetSuggestionsAsync(string token, string language, string searchTerm, int size, string searchArea)
        {
            if (searchArea == SearchConstants.SearchArea.StockLevel)
            {
                return await _stockLevelsSearchSuggestionsService.GetSuggestionsAsync(
                    token,
                    language,
                    searchTerm,
                    size);
            }
            else
            {
                return await _productsSearchSugestionsService.GetSuggestionsAsync(
                    token,
                    language,
                    searchTerm,
                    size);
            }
        }
    }
}
