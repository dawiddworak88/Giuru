using Buyer.Web.Areas.Products.DomainModels;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories.Inventories
{
    public interface IInventoryRepository
    {
        Task<PagedResults<IEnumerable<InventorySum>>> GetAvailbleProductsInventory(
            string language,
            int pageIndex,
            int itemsPerPage,
            string token);
        Task<IEnumerable<InventorySum>> GetAvailbleProductsInventoryByIds(string token, string language, IEnumerable<Guid> ids);
        Task<InventorySum> GetAvailbleProductByProductIdAsync(string token, string language, Guid id);
    }
}
