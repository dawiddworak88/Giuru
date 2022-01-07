using Catalog.BackgroundTasks.IntegrationEvents;
using Catalog.BackgroundTasks.Services.Products;
using Foundation.EventBus.Abstractions;
using System.Threading.Tasks;

namespace Catalog.BackgroundTasks.IntegrationEventsHandlers
{
    public class RebuildCatalogSearchIndexIntegrationEventHandler : IIntegrationEventHandler<RebuildCatalogSearchIndexIntegrationEvent>
    {
        private readonly IProductsService productsService;

        public RebuildCatalogSearchIndexIntegrationEventHandler(
            IProductsService productsService)
        {
            this.productsService = productsService;
        }

        /// <summary>
        /// Integration event handler which starts the rebuild of catalog index
        /// </summary>
        /// <param name="event">
        /// Integration event message which is sent by the
        /// catalog.api once seller triggered the rebuild.
        /// </param>
        /// <returns></returns>
        public async Task Handle(RebuildCatalogSearchIndexIntegrationEvent @event)
        {
            if (@event.OrganisationId.HasValue)
            {
                await this.productsService.IndexAllAsync(@event.OrganisationId);
            }
        }
    }
}
