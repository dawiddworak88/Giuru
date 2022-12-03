using Foundation.EventBus.Abstractions;
using Inventory.Api.IntegrationEvents;
using Inventory.Api.Services.OutletItems;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Api.IntegrationEventsHandlers
{
    public class UpdatedOutletIntegrationEventHandler : IIntegrationEventHandler<BasketCheckoutOutletProductsIntegrationEvent>
    {
        private readonly IOutletService outletService;

        public UpdatedOutletIntegrationEventHandler(
            IOutletService outletService)
        {
            this.outletService = outletService;
        }

        public async Task Handle(BasketCheckoutOutletProductsIntegrationEvent @event)
        {
            using var source = new ActivitySource(this.GetType().Name);
            using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {@event.GetType().Name}");

            if (@event.Items.Any())
            {
                foreach (var item in @event.Items)
                {
                    await this.outletService.UpdateOutletQuantity(item.ProductId, item.BookedQuantity);
                }
            }
        }
    }
}