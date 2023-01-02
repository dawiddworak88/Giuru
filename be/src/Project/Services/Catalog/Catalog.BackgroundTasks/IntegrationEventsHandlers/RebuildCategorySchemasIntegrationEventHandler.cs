using Catalog.BackgroundTasks.IntegrationEvents;
using Catalog.BackgroundTasks.Services.CategorySchemas;
using Foundation.EventBus.Abstractions;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Catalog.BackgroundTasks.IntegrationEventsHandlers
{
    public class RebuildCategorySchemasIntegrationEventHandler : IIntegrationEventHandler<RebuildCategorySchemasIntegrationEvent>
    {
        private readonly ICategorySchemaService categorySchemaService;

        public RebuildCategorySchemasIntegrationEventHandler(
            ICategorySchemaService categorySchemaService)
        {
            this.categorySchemaService = categorySchemaService;
        }

        /// <summary>
        /// Integration event handler which starts the rebuild of category schemas
        /// </summary>
        /// <param name="event">
        /// Integration event message which is sent by the
        /// catalog.api once product attribute has been updated.
        /// </param>
        /// <returns></returns>
        public async Task Handle(RebuildCategorySchemasIntegrationEvent @event)
        {
            using var source = new ActivitySource(this.GetType().Name);
            using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {@event.GetType().Name}");

            await this.categorySchemaService.RebuildCategorySchemasAsync();
        }
    }
}
