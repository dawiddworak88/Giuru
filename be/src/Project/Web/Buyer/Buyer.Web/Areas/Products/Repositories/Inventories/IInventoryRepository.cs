using Buyer.Web.Areas.Products.DomainModels;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories.Inventories
{
    public interface IInventoryRepository
    {
        Task<PagedResults<IEnumerable<InventorySum>>> GetAvailableProductsInventory(
            string language,
            int pageIndex,
            int itemsPerPage,
            string token);
        Task<IEnumerable<InventorySum>> GetAvailableProductsInventoryByIds(string token, string language, IEnumerable<Guid> ids);
        Task<IEnumerable<InventorySuggestion>> GetAvailableProductsInventorySuggestions(string token, string language, string searchTerm, int size);
    }
}
