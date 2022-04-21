using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Inventory.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Inventory.Repositories.Inventories
{
    public interface IInventoryRepository
    {
        Task<PagedResults<IEnumerable<InventoryItem>>> GetInventoryProductsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<InventoryItem> GetInventoryProductAsync(string token, string language, Guid? id);
        Task<IEnumerable<InventoryItem>> GetAllProductsAsync(string token, string language, IEnumerable<Guid> inventoryIds);
        Task<IEnumerable<InventoryItem>> GetAllProductsAsync(string token, string language);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<Guid> SaveAsync(string token, string language, Guid? id, Guid? WarehouseId, Guid? ProductId, string ProductName, string ProductSku, int Quantity, string ean, int? RestockableInDays, int? AvailableQuantity, DateTime? ExpectedDelivery, Guid? OrganisationId);
    }
}
