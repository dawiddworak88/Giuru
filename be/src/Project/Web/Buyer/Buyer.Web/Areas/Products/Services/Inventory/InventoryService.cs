using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly LinkGenerator _linkGenerator;

        public InventoryService(IInventoryRepository inventoryRepository, LinkGenerator linkGenerator) 
        {
            _inventoryRepository = inventoryRepository;
            _linkGenerator = linkGenerator;
        }

        public async Task<IEnumerable<ProductSuggestionViewModel>> GetInventoryProductSuggestionsAsync(string searchTerm, int size, string language, string token, string searchArea)
        {
            var suggestions = await _inventoryRepository.GetAvailbleProductsInventorySuggestions(token, language, searchTerm, size);

            return suggestions.Select(x => new ProductSuggestionViewModel
            {
                Name = x.Name,
                Url = _linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, x.Id }),
            });
        }
    }
}
