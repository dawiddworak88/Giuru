using Buyer.Web.Areas.Products.Services.Inventories;
using Buyer.Web.Areas.Products.Services.SearchSuggestions.BaseSearchSuggestions;
using Buyer.Web.Areas.Products.ViewModels.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.SearchSuggestions.StockLevelsSearchSuggetions
{
    public class StockLevelsSearchSuggestionsService : IBaseSearchSuggestionsService
    {
        private readonly IInventoryService _inventoryService;

        public StockLevelsSearchSuggestionsService(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task<IEnumerable<ProductSuggestionViewModel>> GetSuggestionsAsync(string token, string language, string searchTerm, int size)
        {
            return await _inventoryService.GetInventoryProductSuggestionsAsync(
                searchTerm,
                size,
                language,
                token);
        }
    }
}
