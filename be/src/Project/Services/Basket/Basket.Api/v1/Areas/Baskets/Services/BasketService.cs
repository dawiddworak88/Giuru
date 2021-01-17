using Basket.Api.v1.Areas.Baskets.IntegrationEvents;
using Basket.Api.v1.Areas.Baskets.Models;
using Basket.Api.v1.Areas.Baskets.Repositories;
using Basket.Api.v1.Areas.Baskets.ResultModels;
using Foundation.EventBus.Abstractions;
using Foundation.Extensions.ExtensionMethods;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.v1.Areas.Baskets.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IEventBus eventBus;
        private readonly ILogger<BasketService> logger;

        public BasketService(
            IBasketRepository basketRepository,
            IEventBus eventBus,
            ILogger<BasketService> logger)
        {
            this.basketRepository = basketRepository;
            this.eventBus = eventBus;
            this.logger = logger;
        }

        public async Task CheckoutAsync(CheckoutBasketServiceModel checkoutBasketServiceModel)
        {
            var basket = await this.basketRepository.GetBasketAsync(checkoutBasketServiceModel.BasketId.Value);

            if (basket == null)
            {
                throw new ArgumentNullException();
            }

            var eventMessage = new BasketCheckoutAcceptedIntegrationEvent
            {
                ClientId = checkoutBasketServiceModel.ClientId,
                SellerId = checkoutBasketServiceModel.OrganisationId,
                Basket = basket
            };

            try
            {
                this.eventBus.Publish(eventMessage);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "ERROR Publishing integration event: {IntegrationEventId} from {AppName}", eventMessage.Id, typeof(Program).Namespace);

                throw;
            }
        }

        public async Task<BasketResultModel> UpdateAsync(UpdateBasketServiceModel serviceModel)
        {
            var basketModel = new BasketModel
            { 
                Id = serviceModel.Id,
                Items = serviceModel.Items.OrEmptyIfNull().Select(x => new BasketItemServiceModel 
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    DeliveryFrom = x.DeliveryFrom,
                    DeliveryTo = x.DeliveryTo,
                    MoreInfo = x.MoreInfo
                })
            };

            var result = await this.basketRepository.UpdateBasketAsync(basketModel);

            return new BasketResultModel
            { 
                Id = result.Id,
                Items = result.Items.OrEmptyIfNull().Select(x => new BasketItemResultModel
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
