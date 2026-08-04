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
        private readonly IEventBus _eventBus;

        public UpdatedInventoryIntegrationEventHandler(
            IInventoryService inventoryService,
            IEventBus eventBus)
        {
            _inventoryService = inventoryService;
            _eventBus = eventBus;
        }

        public async Task Handle(BasketCheckoutStockProductsIntegrationEvent @event)
        {
            using var source = new ActivitySource(this.GetType().Name);
            using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {@event.GetType().Name}");

            if (@event.Items.Any())
            {
                var inventoryUpdateResults = new List<InventoryUpdateResultServiceModel>();

                foreach (var item in @event.Items)
                {
                    var updateResult = await _inventoryService.UpdateInventoryQuantity(item.ProductId, item.BookedQuantity);

                    inventoryUpdateResults.Add(updateResult);
                }

                if (inventoryUpdateResults.Any(x => x.WentOutOfStock))
                {
                    var productsSoldOut = new ProductsSoldOutIntegrationEvent
                    {
                        SoldOutProductIds = inventoryUpdateResults
                            .Where(x => x.WentOutOfStock)
                            .Select(x => x.ProductId)
                    };

                    _eventBus.Publish(productsSoldOut);
                }
            }
        }
    }
}
