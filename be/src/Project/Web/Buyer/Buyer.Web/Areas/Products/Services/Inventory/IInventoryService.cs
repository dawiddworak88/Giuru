using Buyer.Web.Areas.Products.ViewModels.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.Inventory
{
    public interface IInventoryService
    {
        Task<IEnumerable<ProductSuggestionViewModel>> GetInventoryProductSuggestionsAsync(string searchTerm, int size, string language, string token, string searchArea);
    }
}
