using Foundation.EventBus.Abstractions;
using Inventory.Api.IntegrationEvents;
using Inventory.Api.Services.OutletItems;
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