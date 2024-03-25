using Seller.Web.Areas.Orders.DomainModels;
using System;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.Repositories.OrderItems
{
    public interface IOrderItemsRepository
    {
        Task<OrderItem> GetAsync(string token, string language, Guid? id);
        Task<OrderItemStatusChanges> GetStatusChangesAsync(string token, string language, Guid? id);
        Task UpdateStatusAsync(string token, string language, Guid? id, Guid? orderItemStatusId, string expectedDateOfProductOnStock);
    }
}
