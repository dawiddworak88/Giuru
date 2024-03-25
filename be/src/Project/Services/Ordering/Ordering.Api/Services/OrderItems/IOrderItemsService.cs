using Ordering.Api.ServicesModels.OrderItems;
using System.Threading.Tasks;

namespace Ordering.Api.Services.OrderItems
{
    public interface IOrderItemsService
    {
        Task<OrderItemServiceModel> GetAsync(GetOrderItemServiceModel model);
        Task<OrderItemStatusChangesServiceModel> GetStatusChangesAsync(GetOrderItemStatusChangesServiceModel model);
        Task UpdateStatusAsync(UpdateOrderItemStatusServiceModel model);
    }
}
