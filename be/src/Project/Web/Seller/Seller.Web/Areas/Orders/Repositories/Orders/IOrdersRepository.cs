using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Orders.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.Repositories.Orders
{
    public interface IOrdersRepository
    {
        Task<Order> GetOrderAsync(string token, string language, Guid? id);
        Task<OrderItem> GetOrderItemAsync(string token, string language, Guid? id);
        Task<OrderItemStatusesHistory> GetOrderItemStatusesAsync(string token, string language, Guid? id);
        Task<PagedResults<IEnumerable<Order>>> GetOrdersAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<IEnumerable<OrderStatus>> GetOrderStatusesAsync(string token, string language);
        Task<Guid> SaveOrderStatusAsync(string token, string language, Guid orderId, Guid orderStatusId);
        Task UpdateOrderItemStatusAsync(string token, string language, Guid id, Guid orderStatusId, string orderStatusComment);
    }
}