using Basket.Api.ServicesModels;
using Basket.Api.Repositories;
using Foundation.EventBus.Abstractions;
using Foundation.Extensions.ExtensionMethods;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Basket.Api.RepositoriesModels;
using Basket.Api.IntegrationEvents;
using Basket.Api.IntegrationEventsModels;
using Foundation.EventLog.Repositories;
using Foundation.EventLog.Definitions;

namespace Basket.Api.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IEventBus eventBus;
        private readonly IEventLogRepository eventLogRepository;

        public BasketService(
            IBasketRepository basketRepository,
            IEventBus eventBus,
            IEventLogRepository eventLogRepository)
        {
            this.basketRepository = basketRepository;
            this.eventBus = eventBus;
            this.eventLogRepository = eventLogRepository;
        }

        public async Task CheckoutAsync(CheckoutBasketServiceModel checkoutBasketServiceModel)
        {
            var basket = await this.basketRepository.GetBasketAsync(checkoutBasketServiceModel.BasketId.Value);

            if (basket == null)
            {
                throw new ArgumentNullException();
            }

            var message = new BasketCheckoutAcceptedIntegrationEvent
            {
                Language = checkoutBasketServiceModel.Language,
                OrganisationId = checkoutBasketServiceModel.OrganisationId,
                Username = checkoutBasketServiceModel.Username,
                ClientId = checkoutBasketServiceModel.ClientId,
                SellerId = checkoutBasketServiceModel.OrganisationId,
                Basket = new BasketEventModel
                {
                    Id = basket.Id,
                    Items = basket.Items.Select(x => new BasketItemEventModel 
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        DeliveryFrom = x.DeliveryFrom,
                        DeliveryTo = x.DeliveryTo,
                        MoreInfo = x.MoreInfo
                    })
                }
            };

            await this.eventLogRepository.SaveAsync(message, message.GetType().Name, EventStates.New, message.Source, message.IpAddress);

            this.eventBus.Publish(message);
        }

        public async Task<BasketServiceModel> UpdateAsync(UpdateBasketServiceModel serviceModel)
        {
            var basketModel = new BasketRepositoryModel
            { 
                Id = serviceModel.Id,
                Items = serviceModel.Items.OrEmptyIfNull().Select(x => new BasketItemRepositoryModel 
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    DeliveryFrom = x.DeliveryFrom,
                    DeliveryTo = x.DeliveryTo,
                    MoreInfo = x.MoreInfo
                })
            };

            var result = await this.basketRepository.UpdateBasketAsync(basketModel);

            return new BasketServiceModel
            { 
                Id = result.Id,
                Items = result.Items.OrEmptyIfNull().Select(x => new BasketItemServiceModel
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    DeliveryFrom = x.DeliveryFrom,
                    DeliveryTo = x.DeliveryTo,
                    MoreInfo = x.MoreInfo
                })
            };
        }
    }
}
