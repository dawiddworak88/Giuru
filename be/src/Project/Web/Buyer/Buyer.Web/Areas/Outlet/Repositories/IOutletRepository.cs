using Buyer.Web.Areas.Outlet.DomainModels;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Outlet.Repositories
{
    public interface IOutletRepository
    {
        Task<PagedResults<IEnumerable<OutletItem>>> GetOutletProductsAsync(string language, int pageIndex, int itemsPerPage, string token);
    }
}
