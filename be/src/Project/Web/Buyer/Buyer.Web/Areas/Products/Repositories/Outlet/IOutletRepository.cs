using Buyer.Web.Areas.Products.DomainModels;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories
{
    public interface IOutletRepository
    {
        Task<PagedResults<IEnumerable<OutletSum>>> GetOutletProductsAsync(string language, int pageIndex, int itemsPerPage, string token);
        Task<IEnumerable<OutletSum>> GetOutletProductsByIdsAsync(string token, string language, IEnumerable<Guid> ids);
        Task<OutletSum> GetOutletProductByProductIdAsync(string token, string language, Guid id);
    }
}
