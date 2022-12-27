using Foundation.GenericRepository.Paginations;
using Ordering.Api.ServicesModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.Api.Services
{
    public interface IOrdersService
    {
        PagedResults<IEnumerable<OrderServiceModel>> Get(GetOrdersServiceModel model);
        PagedResults<IEnumerable<OrderServiceModel>> Get(GetOrdersByIdsServiceModel model);
        Task<OrderServiceModel> GetAsync(GetOrderServiceModel model);
        Task CheckoutAsync(CheckoutBasketServiceModel serviceModel);
        Task<IEnumerable<OrderStatusServiceModel>> GetOrderStatusesAsync(GetOrderStatusesServiceModel serviceModel);
        Task<OrderServiceModel> SaveOrderStatusAsync(UpdateOrderStatusServiceModel serviceModel);
        Task UpdateOrderItemStatusAsync(UpdateOrderItemStatusServiceModel model);
        Task SyncOrderItemsStatusesAsync(UpdateOrderItemsStatusesServiceModel model);
        Task SyncOrderLinesStatusesAsync(UpdateOrderLinesStatusesServiceModel model);
        Task<OrderItemServiceModel> GetAsync(GetOrderItemServiceModel model);
        Task<OrderItemStatusChangesServiceModel> GetAsync(GetOrderItemStatusChangesServiceModel model);
        Task<PagedResults<IEnumerable<OrderFileServiceModel>>> GetOrderFilesAsync(GetOrderFilesServiceModel model);
    }
}
