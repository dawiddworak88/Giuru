using Buyer.Web.Areas.Products.ViewModels.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.SearchSuggestions.ProductsSearchSuggestions
{
    public interface IProductsSearchSuggestionsService
    {
        Task<IEnumerable<ProductSuggestionViewModel>> GetSuggestionsAsync(string token, string language, string searchTerm, int size);
    }
}
