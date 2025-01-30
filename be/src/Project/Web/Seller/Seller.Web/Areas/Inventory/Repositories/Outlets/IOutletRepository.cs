using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Inventory.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Inventory.Repositories
{
    public interface IOutletRepository
    {
        Task<PagedResults<IEnumerable<OutletItem>>> GetAsync(
            string token,
            string language, 
            string searchTerm, 
            int pageIndex, 
            int itemsPerPage, 
            string orderBy);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<OutletItem> GetOutletItemAsync(string token, string language, Guid? id);
        Task<Guid> SaveAsync(
            string token, string language, Guid? id, Guid? warehouseId, Guid? productId, string productName, string productSku, double quantity, string title, string description, string ean, double? availableQuantity, Guid? organisationId);
        Task<IEnumerable<OutletItem>> GetOutletProductsByProductsIdAsync(string token, string language, IEnumerable<Guid> ids);
    }
}
