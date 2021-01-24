using Catalog.BackgroundTasks.IntegrationEvents;
using Catalog.BackgroundTasks.Services.Products;
using Foundation.EventBus.Abstractions;
using Foundation.EventLog.Definitions;
using Foundation.EventLog.Repositories;
using System.Threading.Tasks;

namespace Catalog.BackgroundTasks.IntegrationEventsHandlers
{
    public class RebuildCatalogSearchIndexIntegrationEventHandler : IIntegrationEventHandler<RebuildCatalogSearchIndexIntegrationEvent>
    {
        private readonly IProductsService productsService;
        private readonly IEventLogRepository eventLogRepository;

        public RebuildCatalogSearchIndexIntegrationEventHandler(
            IProductsService productsService,
            IEventLogRepository eventLogRepository)
        {
            this.productsService = productsService;
            this.eventLogRepository = eventLogRepository;
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
            await this.eventLogRepository.SaveAsync(@event, @event.GetType().Name, EventStates.InProgress, null, null);

            if (@event.OrganisationId.HasValue)
            {
                await this.productsService.IndexAllAsync(@event.OrganisationId);
            }

            await this.eventLogRepository.SaveAsync(@event, @event.GetType().Name, EventStates.Processed, null, null);
        }
    }
}
