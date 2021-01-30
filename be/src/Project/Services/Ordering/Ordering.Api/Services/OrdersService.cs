using Foundation.EventBus.Abstractions;
using Foundation.EventLog.Definitions;
using Foundation.EventLog.Repositories;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Microsoft.EntityFrameworkCore;
using Ordering.Api.Infrastructure;
using Ordering.Api.Infrastructure.Orders.Definitions;
using Ordering.Api.Infrastructure.Orders.Entities;
using Ordering.Api.IntegrationEvents;
using Ordering.Api.ServicesModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Api.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly OrderingContext context;
        private readonly IEventBus eventBus;
        private readonly IEventLogRepository eventLogRepository;

        public OrdersService(
            OrderingContext context,
            IEventBus eventBus,
            IEventLogRepository eventLogRepository)
        {
            this.context = context;
            this.eventBus = eventBus;
            this.eventLogRepository = eventLogRepository;
        }

        public async Task CheckoutAsync(CheckoutBasketServiceModel serviceModel)
        {
            var order = new Order
            {
                OrderStateId = OrderStatesConstants.NewId,
                OrderStatusId = OrderStatusesConstants.NewId,
                ClientId = serviceModel.ClientId,
                SellerId = serviceModel.SellerId,
                BillingAddressId = serviceModel.BillingAddressId,
                ShippingAddressId = serviceModel.ShippingAddressId,
                MoreInfo = serviceModel.MoreInfo,
                ExpectedDeliveryDate = serviceModel.ExpectedDeliveryDate,
                IpAddress = serviceModel.IpAddress
            };

            this.context.Orders.Add(order.FillCommonProperties());

            foreach (var basketItem in serviceModel.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = basketItem.ProductId.Value,
                    Quantity = basketItem.Quantity,
                    ExpectedDeliveryFrom = basketItem.ExpectedDeliveryFrom,
                    ExpectedDeliveryTo = basketItem.ExpectedDeliveryTo,
                    MoreInfo = basketItem.MoreInfo                    
                };

                this.context.OrderItems.Add(orderItem.FillCommonProperties());
            }

            await this.context.SaveChangesAsync();

            var message = new OrderStartedIntegrationEvent
            { 
                BasketId =  serviceModel.BasketId
            };

            await this.eventLogRepository.SaveAsync(message, EventStates.New);

            this.eventBus.Publish(message);
        }

        public async Task<PagedResults<IEnumerable<OrderServiceModel>>> GetAsync(GetOrdersServiceModel model)
        {
            var orders = from c in this.context.Orders
                         join os in this.context.OrderStatuses on c.OrderStatusId equals os.Id
                         join ost in this.context.OrderStatusTranslations on os.Id equals ost.OrderStatusId
                         where ost.Language == model.Language && c.SellerId == model.OrganisationId.Value && c.IsActive
                         select new OrderServiceModel
                         {
                             Id = c.Id,
                             ClientId = c.ClientId.Value,
                             BillingAddressId = c.BillingAddressId,
                             ShippingAddressId = c.ShippingAddressId,
                             MoreInfo = c.MoreInfo,
                             ExpectedDeliveryDate = c.ExpectedDeliveryDate,
                             Reason = c.Reason,
                             OrderStateId = c.OrderStateId,
                             OrderStatusId = c.OrderStatusId,
                             OrderStatusName = ost.Name,
                             OrderItems = c.OrderItems.Select(x => new OrderItemServiceModel
                             {
                                 ProductId = x.ProductId,
                                 Quantity = x.Quantity,
                                 ExpectedDeliveryFrom = x.ExpectedDeliveryFrom,
                                 ExpectedDeliveryTo = x.ExpectedDeliveryTo,
                                 MoreInfo = x.MoreInfo,
                                 LastModifiedDate = x.LastModifiedDate,
                                 CreatedDate = x.CreatedDate
                             }),
                             LastModifiedDate = c.LastModifiedDate,
                             CreatedDate = c.CreatedDate
                         };

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                orders = orders.Where(x => x.Id.ToString().StartsWith(model.SearchTerm));
            }

            orders = orders.ApplySort(model.OrderBy);

            return orders.PagedIndex(new Pagination(orders.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<OrderServiceModel> GetAsync(GetOrderServiceModel model)
        {
            var orders = from c in this.context.Orders
                         join os in this.context.OrderStatuses on c.OrderStatusId equals os.Id
                         join ost in this.context.OrderStatusTranslations on os.Id equals ost.OrderStatusId
                          where ost.Language == model.Language && c.SellerId == model.OrganisationId.Value && c.IsActive
                          select new OrderServiceModel
                          {
                              Id = c.Id,
                              ClientId = c.ClientId.Value,
                              BillingAddressId = c.BillingAddressId,
                              ShippingAddressId = c.ShippingAddressId,
                              MoreInfo = c.MoreInfo,
                              ExpectedDeliveryDate = c.ExpectedDeliveryDate,
                              Reason = c.Reason,
                              OrderStateId = c.OrderStateId,
                              OrderStatusId = c.OrderStatusId,
                              OrderStatusName = ost.Name,
                              OrderItems = c.OrderItems.Select(x => new OrderItemServiceModel 
                              { 
                                ProductId = x.ProductId,
                                Quantity = x.Quantity,
                                ExpectedDeliveryFrom = x.ExpectedDeliveryFrom,
                                ExpectedDeliveryTo = x.ExpectedDeliveryTo,
                                MoreInfo = x.MoreInfo,
                                LastModifiedDate = x.LastModifiedDate,
                                CreatedDate = x.CreatedDate
                              }),
                              LastModifiedDate = c.LastModifiedDate,
                              CreatedDate = c.CreatedDate
                          };

            return await orders.FirstOrDefaultAsync();
        }
    }
}
