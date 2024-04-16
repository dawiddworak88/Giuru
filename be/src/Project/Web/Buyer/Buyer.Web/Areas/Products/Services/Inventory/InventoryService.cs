using Buyer.Web.Areas.Products.Repositories.Inventories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository) 
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<IEnumerable<string>> GetInventoryProductSuggestionsAsync(string searchTerm, int size, string language, string token, string searchArea)
        {
            return await _inventoryRepository.GetAvailbleProductsInventorySuggestions(token, language, searchTerm, size);
        }
    }
}
