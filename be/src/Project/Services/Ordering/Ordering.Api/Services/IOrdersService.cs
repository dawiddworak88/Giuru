using Foundation.GenericRepository.Paginations;
using Ordering.Api.ServicesModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.Api.Services
{
    public interface IOrdersService
    {
        Task<PagedResults<IEnumerable<OrderServiceModel>>> GetAsync(GetOrdersServiceModel model);
        Task<OrderServiceModel> GetAsync(GetOrderServiceModel model);
        Task CheckoutAsync(CheckoutBasketServiceModel serviceModel);
        Task<IEnumerable<OrderStatusServiceModel>> GetOrderStatusesAsync(GetOrderStatusesServiceModel serviceModel);
        Task<OrderServiceModel> SaveOrderStatusAsync(UpdateOrderStatusServiceModel serviceModel);
        Task UpdateOrderItemStatusAsync(UpdateOrderItemStatusServiceModel model);
        Task SyncOrderItemsStatusesAsync(UpdateOrderItemsStatusesServiceModel model);
        Task<OrderItemServiceModel> GetAsync(GetOrderItemServiceModel model);
    }
}
