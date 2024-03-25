using Ordering.Api.ServicesModels;
using System.Threading.Tasks;

namespace Ordering.Api.Services.OrderItems
{
    public interface IOrderItemsService
    {
        Task<OrderItemServiceModel> GetAsync(GetOrderItemServiceModel model);
        Task UpdateOrderItemStatusAsync(UpdateOrderItemStatusServiceModel model);
    }
}
