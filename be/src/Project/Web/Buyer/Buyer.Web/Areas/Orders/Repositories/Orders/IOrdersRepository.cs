using Buyer.Web.Areas.Orders.DomainModels;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.Repositories
{
    public interface IOrdersRepository
    {
        Task<Order> GetOrderAsync(string token, string language, Guid? id);
        Task<IEnumerable<OrderStatus>> GetOrderStatusesAsync(string token, string language);
        Task<PagedResults<IEnumerable<Order>>> GetOrdersAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<Guid> SaveOrderStatusAsync(string token, string language, Guid orderId, Guid orderStatusId);
    }
}
