using Foundation.EventBus.Abstractions;
using Ordering.Api.IntegrationEvents;
using Ordering.Api.Services;
using Ordering.Api.ServicesModels;
using Ordering.Api.Validators;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Api.v1.Areas.Orders.IntegrationEventsHandlers
{
    public class BasketCheckoutAcceptedIntegrationEventHandler : IIntegrationEventHandler<BasketCheckoutAcceptedIntegrationEvent>
    {
        private readonly IOrdersService ordersService;

        public BasketCheckoutAcceptedIntegrationEventHandler(
            IOrdersService ordersService)
        {
            this.ordersService = ordersService;
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
            using var source = new ActivitySource(this.GetType().Name);
            using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {@event.GetType().Name}");

            var serviceModel = new CheckoutBasketServiceModel
            {
                ClientId = @event.ClientId,
                ClientName = @event.ClientName,
                SellerId = @event.SellerId,
                BasketId = @event.BasketId,
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
                ShippingCountryId = @event.ShippingCountryId,
                ShippingFirstName = @event.ShippingFirstName,
                ShippingLastName = @event.ShippingLastName,
                ShippingPhoneNumber = @event.ShippingPhoneNumber,
                ShippingPostCode = @event.ShippingPostCode,
                ShippingRegion = @event.ShippingRegion,
                ShippingStreet = @event.ShippingStreet,
                ExternalReference = @event.ExternalReference,
                MoreInfo = @event.MoreInfo,
                IpAddress = @event.IpAddress,
                HasCustomOrder = @event.HasCustomOrder,
                Attachments = @event.Attachments,
                Items = @event.Basket?.Items?.Select(x => new CheckoutBasketItemServiceModel
                {
                    ProductId = x.ProductId,
                    ProductSku = x.ProductSku,
                    ProductName = x.ProductName,
                    PictureUrl = x.PictureUrl,
                    Quantity = x.Quantity,
                    StockQuantity = x.StockQuantity,
                    OutletQuantity = x.OutletQuantity,
                    ExternalReference = x.ExternalReference,
                    MoreInfo = x.MoreInfo
                }),
                Language = @event.Language
            };

            var validator = new CheckoutBasketServiceModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await this.ordersService.CheckoutAsync(serviceModel);
            }
        }
    }
}
