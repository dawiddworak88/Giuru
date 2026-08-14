using Foundation.EventBus.Abstractions;
using Inventory.Api.IntegrationEvents;
using Inventory.Api.Services.InventoryItems;
using Inventory.Api.ServicesModels.InventoryServiceModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Api.IntegrationEventsHandlers
{
    public class UpdatedInventoryIntegrationEventHandler : IIntegrationEventHandler<BasketCheckoutStockProductsIntegrationEvent>
    {
        private readonly IInventoryService _inventoryService;
        private ILogger<UpdatedInventoryIntegrationEventHandler> _logger;
        private readonly IEventBus _eventBus;

        public UpdatedInventoryIntegrationEventHandler(
            IInventoryService inventoryService,
            ILogger<UpdatedInventoryIntegrationEventHandler> logger,
            IEventBus eventBus)
        {
            _inventoryService = inventoryService;
            _logger = logger;
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
                    if (item.ProductId is null || item.BookedQuantity <= 0)
                    {
                        _logger.LogWarning(
                            "Skipping inventory update. Invalid item: ProductId={ProductId}, BookedQuantity={BookedQuantity}",
                            item.ProductId, item.BookedQuantity);

                        continue;
                    }

                    var updateResult = await _inventoryService.UpdateInventoryQuantity(item.ProductId.Value, item.BookedQuantity);

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

                    try
                    {
                        _eventBus.Publish(productsSoldOut);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to publish ProductsSoldOutIntegrationEvent for products: {ProductIds}",
                            string.Join(",", productsSoldOut.SoldOutProductIds));
                    }
                }
            }
        }
    }
}
