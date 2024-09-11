using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Areas.Shared.Definitions.Products;
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.Inventories
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IProductsService _productsService;

        public InventoryService(
            IInventoryRepository inventoryRepository,
            LinkGenerator linkGenerator,
            IProductsService productsService) 
        {
            _inventoryRepository = inventoryRepository;
            _linkGenerator = linkGenerator;
            _productsService = productsService;
        }

        public async Task<IEnumerable<ProductSuggestionViewModel>> GetInventoryProductSuggestionsAsync(string searchTerm, int size, string language, string token)
        {
            var suggestions = await _inventoryRepository.GetAvailableInventoryProductsSuggestions(token, language, searchTerm, size);

            var products = await _productsService.GetProductsAsync(
                suggestions.Select(x => x.Id),
                null,
                null,
                language,
                null,
                true,
                PaginationConstants.DefaultPageIndex,
                ProductConstants.ProductsCatalogPaginationPageSize,
                token);

            return suggestions.Select(x => new ProductSuggestionViewModel
            {
                Name = x.Name,
                Attributes = products?.Data?.FirstOrDefault(y => y.Id == x.Id)?.ProductAttributes,
                Sku = x.Sku,
                Url = _linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, x.Id }),
            });
        }
    }
}
