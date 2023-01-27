using Analytics.Api.IntegrationEvents;
using Analytics.Api.Services.SalesAnalytics;
using Analytics.Api.ServicesModels.SalesAnalytics;
using Analytics.Api.Shared.Repositories.AccessToken;
using Foundation.EventBus.Abstractions;
using Foundation.Extensions.ExtensionMethods;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Analytics.Api.IntegrationEventHandlers
{
    public class OrderStartedIntegrationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
    {
        private readonly IAccessTokenRepository _accessTokenRepository;
        private readonly ISalesService _salesService;

        public OrderStartedIntegrationEventHandler(
            IAccessTokenRepository accessTokenRepository,
            ISalesService salesService)
        {
            _accessTokenRepository = accessTokenRepository;
            _salesService = salesService;
        }
        public async Task Handle(OrderStartedIntegrationEvent @event)
        {
            var accessToken = await _accessTokenRepository.GetAccessTokenAsync();

            var salesAnalyticsItem = new CreateSalesAnalyticsServiceModel
            {
                ClientId = @event.ClientId,
                Token = accessToken,
                CreatedDate = @event.CreatedDate
            };

            var products = new List<CreateSalesAnalyticsProductServiceModel>();

            foreach (var orderItem in @event.OrderItems.OrEmptyIfNull())
            {
                var product = new CreateSalesAnalyticsProductServiceModel
                {
                    Id = orderItem.Id,
                    Quantity = orderItem.Quantity,
                    StockQuantity = orderItem.StockQuantity,
                    OutletQuantity = orderItem.OutletQuantity
                };

                products.Add(product);
            }

            salesAnalyticsItem.Products = products;

            await _salesService.CreateAsync(salesAnalyticsItem);
        }
    }
}
