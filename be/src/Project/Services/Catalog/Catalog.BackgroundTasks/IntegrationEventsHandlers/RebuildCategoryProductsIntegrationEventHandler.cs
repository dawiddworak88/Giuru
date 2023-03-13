using Catalog.BackgroundTasks.IntegrationEvents;
using Catalog.BackgroundTasks.Services.Products;
using Foundation.EventBus.Abstractions;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Catalog.BackgroundTasks.IntegrationEventsHandlers
{
    public class RebuildCategoryProductsIntegrationEventHandler : IIntegrationEventHandler<RebuildCategoryProductsIntegrationEvent>
    {
        private readonly IProductsService _productsService;

        public RebuildCategoryProductsIntegrationEventHandler(
            IProductsService productsService)
        {
            _productsService = productsService;
        }

        /// <summary>
        /// Integration event handler which starts the rebuild of category products index
        /// </summary>
        /// <param name="event">
        /// Integration event message which is sent by the
        /// catalog.api once seller saves the product card
        /// </param>
        /// <returns></returns>
        public async Task Handle(RebuildCategoryProductsIntegrationEvent @event)
        {
            using var source = new ActivitySource(this.GetType().Name);
            using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {@event.GetType().Name}");

            if (@event.OrganisationId.HasValue && @event.CategoryId.HasValue)
            {
                await _productsService.IndexCategoryProducts(@event.CategoryId, @event.OrganisationId);
            }
        }
    }
}
