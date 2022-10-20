using Analytics.Api.IntegrationEvents;
using Analytics.Api.Services.Products;
using Foundation.EventBus.Abstractions;
using System.Threading.Tasks;

namespace Analytics.Api.IntegrationEventsHandlers
{
    public class UpdatedProductIntegrationEventHandler : IIntegrationEventHandler<UpdatedProductIntegrationEvent>
    {
        private readonly IProductService productService;

        public UpdatedProductIntegrationEventHandler(IProductService productService)
        {
            this.productService = productService;
        }

        /// <summary>
        /// Integration event handler which which updates the product
        /// </summary>
        /// <param name="event">
        /// Integration event message which is sent by the catalog.api
        /// </param>
        /// <returns></returns>
        public async Task Handle(UpdatedProductIntegrationEvent @event)
        {
            if (@event.OrganisationId.HasValue)
            {
                await this.productService.UpdateProductAsync(@event.ProductId, @event.ProductName, @event.ProductSku, @event.ProductEan, @event.Language);
            }
        }
    }
}
