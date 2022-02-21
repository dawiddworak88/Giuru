using Foundation.EventBus.Abstractions;
using Inventory.Api.IntegrationEvents;
using Inventory.Api.Services.InventoryItems;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Api.IntegrationEventsHandlers
{
    public class UpdatedInventoryIntegrationEventHandler : IIntegrationEventHandler<BasketCheckoutProductsIntegrationEvent>
    {
        private readonly IInventoryService inventoryService;

        public UpdatedInventoryIntegrationEventHandler(
            IInventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
        }

        public async Task Handle(BasketCheckoutProductsIntegrationEvent @event)
        {
            if (@event.Items.Any())
            {
                foreach (var item in @event.Items)
                {
                    await this.inventoryService.UpdateInventoryBasket(item.ProductId, item.BookedQuantity);
                }
            }
        }
    }
}
