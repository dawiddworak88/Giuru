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
                ClientName = @event.ClientName,
                SellerId = @event.SellerId,
                BasketId = @event.Basket?.Id,
                BillingAddressId = @event.BillingAddressId,
                BillingCity = @event.BillingCity,
                BillingCompany = @event.BillingCompany,
                BillingCountryCode = @event.BillingCountryCode,
                BillingFirstName = @event.BillingFirstName,
                BillingLastName = @event.BillingLastName,
                BillingPhone = @event.BillingPhone,
                BillingPhonePrefix = @event.BillingPhonePrefix,
                BillingPostCode = @event.BillingPostCode,
                BillingRegion = @event.BillingRegion,
                BillingStreet = @event.BillingStreet,
                ShippingAddressId = @event.ShippingAddressId,
                ShippingCity = @event.ShippingCity,
                ShippingCompany = @event.ShippingCompany,
                ShippingCountryCode = @event.ShippingCountryCode,
                ShippingFirstName = @event.ShippingFirstName,
                ShippingLastName = @event.ShippingLastName,
                ShippingPhone = @event.ShippingPhone,
                ShippingPhonePrefix = @event.ShippingPhonePrefix,
                ShippingPostCode = @event.ShippingPostCode,
                ShippingRegion = @event.ShippingRegion,
                ShippingStreet = @event.ShippingStreet,
                ExpectedDeliveryDate = @event.ExpectedDeliveryDate,
                ExternalReference = @event.ExternalReference,
                MoreInfo = @event.MoreInfo,
                IpAddress = @event.IpAddress,
                Items = @event.Basket?.Items?.Select(x => new CheckoutBasketItemServiceModel
                {
                    ProductId = x.ProductId,
                    ProductSku = x.ProductSku,
                    ProductName = x.ProductName,
                    PictureUrl = x.PictureUrl,
                    Quantity = x.Quantity,
                    ExternalReference = x.ExternalReference,
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
