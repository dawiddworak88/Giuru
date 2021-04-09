using Catalog.BackgroundTasks.IntegrationEvents;
using Catalog.BackgroundTasks.Services.CategorySchemas;
using Foundation.EventBus.Abstractions;
using Foundation.EventLog.Definitions;
using Foundation.EventLog.Repositories;
using System.Threading.Tasks;

namespace Catalog.BackgroundTasks.IntegrationEventsHandlers
{
    public class RebuildCategorySchemasIntegrationEventHandler : IIntegrationEventHandler<RebuildCategorySchemasIntegrationEvent>
    {
        private readonly ICategorySchemaService categorySchemaService;
        private readonly IEventLogRepository eventLogRepository;

        public RebuildCategorySchemasIntegrationEventHandler(
            ICategorySchemaService categorySchemaService,
            IEventLogRepository eventLogRepository)
        {
            this.categorySchemaService = categorySchemaService;
            this.eventLogRepository = eventLogRepository;
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
            await this.eventLogRepository.SaveAsync(@event, EventStates.InProgress);

            await this.categorySchemaService.RebuildCategorySchemasAsync();

            await this.eventLogRepository.SaveAsync(@event, EventStates.Processed);
        }
    }
}
