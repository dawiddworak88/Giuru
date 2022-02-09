using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Outlet.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Outlet.Repositories
{
    public interface IOutletRepository
    {
        Task<PagedResults<IEnumerable<OutletItem>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<OutletItem> GetOutletItemAsync(string token, string language, Guid? id);
        Task<Guid> SaveAsync(string token, string language, Guid? id, Guid? ProductId, string ProductName, string ProductSku);
    }
}
