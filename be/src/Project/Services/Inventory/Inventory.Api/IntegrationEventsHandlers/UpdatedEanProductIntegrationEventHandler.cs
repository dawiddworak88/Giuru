using Foundation.EventBus.Abstractions;
using Inventory.Api.IntegrationEvents;
using Inventory.Api.Services;
using System;
using System.Threading.Tasks;

namespace Inventory.Api.IntegrationEventsHandlers
{
    public class UpdatedEanProductIntegrationEventHandler : IIntegrationEventHandler<UpdatedEanProductIntegrationEvent>
    {
        private readonly IInventoryService inventoryService;

        public UpdatedEanProductIntegrationEventHandler(
            IInventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
        }

        /// <summary>
        /// Integration event handler which starts the rebuild of catalog index
        /// </summary>
        /// <param name="event">
        /// Integration event message which is sent by the
        /// catalog.api once seller triggered the rebuild.
        /// </param>
        /// <returns></returns>
        public async Task Handle(UpdatedEanProductIntegrationEvent @event)
        {
            if (@event.ProductId.HasValue)
            {
                await this.inventoryService.UpdateInventoryEan(@event.ProductId, @event.Ean);
            }
        }
    }
}
