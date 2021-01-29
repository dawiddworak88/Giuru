using Foundation.EventBus.Abstractions;
using Foundation.EventLog.Definitions;
using Foundation.EventLog.Repositories;
using Ordering.Api.IntegrationEvents;
using Ordering.Api.Services;
using Ordering.Api.ServicesModels;
using Ordering.Api.Validators;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Api.v1.Areas.Orders.IntegrationEventsHandlers
{
    public class BasketCheckoutAcceptedIntegrationEventHandler : IIntegrationEventHandler<BasketCheckoutAcceptedIntegrationEvent>
    {
        private readonly IOrdersService ordersService;
        private readonly IEventLogRepository eventLogRepository;

        public BasketCheckoutAcceptedIntegrationEventHandler(
            IOrdersService ordersService,
            IEventLogRepository eventLogRepository)
        {
            this.ordersService = ordersService;
            this.eventLogRepository = eventLogRepository;
        }

        /// <summary>
        /// Integration event handler which starts the create order process
        /// </summary>
        /// <param name="event">
        /// Integration event message which is sent by the
        /// basket.api once it has successfully process the 
        /// order items.
        /// </param>
        /// <returns></returns>
        public async Task Handle(BasketCheckoutAcceptedIntegrationEvent @event)
        {
            await this.eventLogRepository.SaveAsync(@event, EventStates.InProgress);

            var serviceModel = new CheckoutBasketServiceModel
            {
                ClientId = @event.ClientId,
                SellerId = @event.SellerId,
                BasketId = @event.Basket?.Id,
                BillingAddressId = @event.BillingAddressId,
                ShippingAddressId = @event.ShippingAddressId,
                ExpectedDeliveryDate = @event.ExpectedDeliveryDate,
                MoreInfo = @event.MoreInfo,
                IpAddress = @event.IpAddress,
                Items = @event.Basket?.Items?.Select(x => new CheckoutBasketItemServiceModel
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    ExpectedDeliveryFrom = x.DeliveryFrom,
                    ExpectedDeliveryTo = x.DeliveryTo,
                    MoreInfo = x.MoreInfo
                })
            };

            var validator = new CheckoutBasketServiceModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await this.ordersService.CheckoutAsync(serviceModel);

                await this.eventLogRepository.SaveAsync(@event, EventStates.Processed);
            }
        }
    }
}
