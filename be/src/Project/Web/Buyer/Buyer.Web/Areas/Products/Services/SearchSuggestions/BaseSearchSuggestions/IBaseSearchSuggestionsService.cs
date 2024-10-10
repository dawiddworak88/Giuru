using Buyer.Web.Areas.Products.ViewModels.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.SearchSuggestions.BaseSearchSuggestions
{
    public interface IBaseSearchSuggestionsService
    {
        Task<IEnumerable<ProductSuggestionViewModel>> GetSuggestionsAsync(string token, string language, string searchTerm, int size);
    }
}
