using Buyer.Web.Areas.Products.ViewModels.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.SearchSuggestions.StockLevelsSearchSuggetions
{
    public interface IStockLevelsSearchSuggestionsService
    {
        Task<IEnumerable<ProductSuggestionViewModel>> GetSuggestionsAsync(string token, string language, string searchTerm, int size);
    }
}
