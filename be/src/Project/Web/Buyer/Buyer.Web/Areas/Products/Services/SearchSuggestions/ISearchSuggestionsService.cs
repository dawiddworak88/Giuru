using Buyer.Web.Areas.Products.ViewModels.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.SearchSuggestions
{
    public interface ISearchSuggestionsService
    {
        Task<IEnumerable<ProductSuggestionViewModel>> GetSuggestionsAsync(string token, string language, string searchTerm, int size, string searchArea);
    }
}
