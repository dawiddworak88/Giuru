using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Orders.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.Repositories.OrderAttributes
{
    public interface IOrderAttributesRepository
    {
        Task DeleteAsync(string token, string language, Guid? id);
        Task<Guid> SaveAsync(string token, string language, Guid? id, string name, string type, bool isOrderItemAttribute);
        Task<IEnumerable<OrderAttribute>> GetAsync(string token, string language);
        Task<OrderAttribute> GetAsync(string token, string language, Guid? id);
        Task<PagedResults<IEnumerable<OrderAttribute>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
    }
}
