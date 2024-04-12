using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Seller.Web.Areas.Orders.DomainModels;

namespace Seller.Web.Areas.Orders.Repositories.OrderAttributeOptions
{
    public interface IOrderAttributeOptionsRepository
    {
        Task<Guid> SaveAsync(string token, string language, Guid? id, string name, Guid? attributeId);
        Task<OrderAttributeOption> GetAsync(string token, string language, Guid? id);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<PagedResults<IEnumerable<OrderAttributeOption>>> GetAsync(string token, string language, Guid? attributeId, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
    }
}
