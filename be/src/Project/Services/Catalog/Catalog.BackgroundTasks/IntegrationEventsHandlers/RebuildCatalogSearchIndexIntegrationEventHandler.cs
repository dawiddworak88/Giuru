using Catalog.BackgroundTasks.IntegrationEvents;
using Catalog.BackgroundTasks.Services.Products;
using Foundation.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Catalog.BackgroundTasks.IntegrationEventsHandlers
{
    public class RebuildCatalogSearchIndexIntegrationEventHandler : IIntegrationEventHandler<RebuildCatalogSearchIndexIntegrationEvent>
    {
        private readonly IProductsService productsService;
        private readonly ILogger _logger;

        public RebuildCatalogSearchIndexIntegrationEventHandler(
            IProductsService productsService,
            ILogger<RebuildCatalogSearchIndexIntegrationEventHandler> logger)
        {
            this.productsService = productsService;
            _logger = logger;
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
            using var source = new ActivitySource(this.GetType().Name);
            using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {@event.GetType().Name}");

            if (@event.OrganisationId.HasValue)
            {
                _logger.LogError($"Rebuild index organisation id is {@event.OrganisationId.Value}");

                await this.productsService.IndexAllAsync(@event.OrganisationId);
            }
            else
            {
                _logger.LogError($"Organisation id by rebuild product index is missing.");
            }
        }
    }
}
