using Catalog.BackgroundTasks.IntegrationEvents;
using Catalog.BackgroundTasks.Services.CategorySchemas;
using Foundation.EventBus.Abstractions;
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
            await this.categorySchemaService.RebuildCategorySchemasAsync();
        }
    }
}
