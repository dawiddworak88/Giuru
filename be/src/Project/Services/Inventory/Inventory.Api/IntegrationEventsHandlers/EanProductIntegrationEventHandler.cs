using Foundation.EventBus.Abstractions;
using Inventory.Api.IntegrationEvents;
using Inventory.Api.Services;
using System;
using System.Threading.Tasks;

namespace Inventory.Api.IntegrationEventsHandlers
{
    public class EanProductIntegrationEventHandler : IIntegrationEventHandler<EanProductIntegrationEvent>
    {
        private readonly IInventoryService inventoryService;

        public EanProductIntegrationEventHandler(
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
        public async Task Handle(EanProductIntegrationEvent @event)
        {
            if (@event.ProductId.HasValue)
            {
                await this.inventoryService.UpdateInventoryEan(@event.ProductId, @event.Ean);
            }
        }
    }
}
