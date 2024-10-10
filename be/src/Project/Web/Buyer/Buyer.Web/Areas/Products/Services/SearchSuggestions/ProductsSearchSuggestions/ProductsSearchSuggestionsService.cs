using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.Services.SearchSuggestions.BaseSearchSuggestions;
using Buyer.Web.Areas.Products.ViewModels.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.SearchSuggestions.ProductsSearchSuggestions
{
    public class ProductsSearchSuggestionsService : IBaseSearchSuggestionsService
    {
        private readonly IProductsService _productsService;

        public ProductsSearchSuggestionsService(IProductsService productsService)
        {
            _productsService = productsService;
        }

        public async Task<IEnumerable<ProductSuggestionViewModel>> GetSuggestionsAsync(string token, string language, string searchTerm, int size)
        {
            var suggestions = await _productsService.GetProductSuggestionsAsync(
                searchTerm,
                size,
                language,
                token);

            return suggestions;
        }
    }
}
