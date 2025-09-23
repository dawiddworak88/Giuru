using Catalog.BackgroundTasks.IntegrationEvents;
using Catalog.BackgroundTasks.Services.Products;
using Catalog.BackgroundTasks.ServicesModels;
using Foundation.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.BackgroundTasks.IntegrationEventsHandlers
{
    public class InventoryProductsAvailableQuantityUpdateIntegrationEventHandler : IIntegrationEventHandler<InventoryProductsAvailableQuantityUpdateIntegrationEvent>
    {
        private readonly IProductsService _productsService;
        private readonly ILogger<InventoryProductsAvailableQuantityUpdateIntegrationEventHandler> _logger;

        public InventoryProductsAvailableQuantityUpdateIntegrationEventHandler(
            IProductsService productsService,
            ILogger<InventoryProductsAvailableQuantityUpdateIntegrationEventHandler> logger)
        {
            _productsService = productsService;
            _logger = logger;
        }

        public async Task Handle(InventoryProductsAvailableQuantityUpdateIntegrationEvent @event)
        {
            using var source = new ActivitySource(this.GetType().Name);
            using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {@event.GetType().Name}");
            
            if (@event.Products == null || !@event.Products.Any())
            {
                _logger.LogWarning("No products available for quantity update at InventoryProductsAvailableQuantityUpdateIntegrationEvent");
                return;
            }

            await _productsService.BatchUpdateStockAvailableQuantitiesAsync(
                    @event.Products.Select(x => new AvailableQuantityServiceModel
                    {
                        ProductSku = x.ProductSku,
                        AvailableQuantity = x.AvailableQuantity
                    }));
        }
    }
}
