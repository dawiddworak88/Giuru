using Foundation.EventBus.Abstractions;
using Foundation.EventLog.Definitions;
using Foundation.EventLog.Repositories;
using Foundation.GenericRepository.Extensions;
using Ordering.Api.Infrastructure;
using Ordering.Api.Infrastructure.Orders.Definitions;
using Ordering.Api.Infrastructure.Orders.Entities;
using Ordering.Api.IntegrationEvents;
using Ordering.Api.ServicesModels;
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
    }
}
