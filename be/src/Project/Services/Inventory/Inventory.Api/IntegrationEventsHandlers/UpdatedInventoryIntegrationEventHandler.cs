using Foundation.EventBus.Abstractions;
using Inventory.Api.IntegrationEvents;
using Inventory.Api.Services.InventoryItems;
using Inventory.Api.ServicesModels.InventoryServiceModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Api.IntegrationEventsHandlers
{
    public class UpdatedInventoryIntegrationEventHandler : IIntegrationEventHandler<BasketCheckoutStockProductsIntegrationEvent>
    {
        private readonly IInventoryService _inventoryService;

        public UpdatedInventoryIntegrationEventHandler(
            IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task Handle(BasketCheckoutStockProductsIntegrationEvent @event)
        {
            using var source = new ActivitySource(this.GetType().Name);
            using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {@event.GetType().Name}");

            if (@event.Items.Any())
            {
                var inventoryUpdateResults = new List<InventoryUpdateResult>();

                foreach (var item in @event.Items)
                {
                    var updateResult = await _inventoryService.UpdateInventoryQuantity(item.ProductId, item.BookedQuantity);

                    inventoryUpdateResults.Add(updateResult);
                }
            }
        }
    }
}
