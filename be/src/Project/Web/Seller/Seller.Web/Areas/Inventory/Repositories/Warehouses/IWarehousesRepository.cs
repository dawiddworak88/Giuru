using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Inventory.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Inventory.Repositories.Warehouses
{
    public interface IWarehousesRepository
    {
        Task<Warehouse> GetWarehouseAsync(string token, string language, Guid? id);
        Task<PagedResults<IEnumerable<Warehouse>>> GetWarehousesAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<IEnumerable<Warehouse>> GetAllWarehousesAsync(string token, string language, IEnumerable<Guid> clientIds);
        Task<Guid> SaveAsync(string token, string language, Guid? id, string name, string location, Guid? OrganisationId);
    }
}
