using Foundation.EventBus.Abstractions;
using Inventory.Api.IntegrationEvents;
using Inventory.Api.Services.InventoryItems;
using Inventory.Api.Services.OutletItems;
using System.Threading.Tasks;

namespace Inventory.Api.IntegrationEventsHandlers
{
    public class UpdatedProductIntegrationEventHandler : IIntegrationEventHandler<UpdatedProductIntegrationEvent>
    {
        private readonly IInventoryService inventoryService;
        private readonly IOutletService outletService;

        public UpdatedProductIntegrationEventHandler(
            IInventoryService inventoryService,
            IOutletService outletService)
        {
            this.inventoryService = inventoryService;
            this.outletService = outletService;
        }

        /// <summary>
        /// Integration event handler which starts the rebuild of catalog index
        /// </summary>
        /// <param name="event">
        /// Integration event message which is sent by the
        /// catalog.api once seller triggered the rebuild.
        /// </param>
        /// <returns></returns>
        public async Task Handle(UpdatedProductIntegrationEvent @event)
        {
            if (@event.OrganisationId.HasValue)
            {
                await this.outletService.UpdateOutletProduct(@event.ProductId, @event.ProductName, @event.ProductSku, @event.OrganisationId);
                await this.inventoryService.UpdateInventoryProduct(@event.ProductId, @event.ProductName, @event.ProductSku, @event.OrganisationId);
            }
        }
    }
}
