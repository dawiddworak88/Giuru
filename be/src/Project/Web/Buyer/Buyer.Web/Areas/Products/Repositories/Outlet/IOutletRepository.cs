using Buyer.Web.Areas.Outlet.DomainModels;
using Buyer.Web.Areas.Products.DomainModels;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories
{
    public interface IOutletRepository
    {
        Task<PagedResults<IEnumerable<OutletSum>>> GetOutletProductsAsync(string language, int pageIndex, int itemsPerPage, string token);
    }
}
