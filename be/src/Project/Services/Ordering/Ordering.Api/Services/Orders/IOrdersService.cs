using Foundation.GenericRepository.Paginations;
using Ordering.Api.ServicesModels.OrderItems;
using Ordering.Api.ServicesModels.Orders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.Api.Services.Orders
{
    public interface IOrdersService
    {
        PagedResults<IEnumerable<OrderServiceModel>> Get(GetOrdersServiceModel model);
        PagedResults<IEnumerable<OrderServiceModel>> Get(GetOrdersByIdsServiceModel model);
        Task<OrderServiceModel> GetAsync(GetOrderServiceModel model);
        Task CheckoutAsync(CheckoutBasketServiceModel serviceModel);
        Task<IEnumerable<OrderStatusServiceModel>> GetOrderStatusesAsync(GetOrderStatusesServiceModel serviceModel);
        Task<OrderServiceModel> SaveOrderStatusAsync(UpdateOrderStatusServiceModel serviceModel);
        Task SyncOrderItemsStatusesAsync(UpdateOrderItemsStatusesServiceModel model);
        Task SyncOrderLinesStatusesAsync(UpdateOrderLinesStatusesServiceModel model);
        Task<PagedResults<IEnumerable<OrderFileServiceModel>>> GetOrderFilesAsync(GetOrderFilesServiceModel model);
        Task MapStatusesToOrderStatusId(Guid? orderId);
    }
}
