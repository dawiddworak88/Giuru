using Buyer.Web.Areas.Products.DomainModels;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories.Inventories
{
    public interface IInventoryRepository
    {
        Task<PagedResults<IEnumerable<InventorySum>>> GetAvailbleProductsInventory(
            string language,
            int? pageIndex,
            int? itemsPerPage,
            string token);
    }
}
