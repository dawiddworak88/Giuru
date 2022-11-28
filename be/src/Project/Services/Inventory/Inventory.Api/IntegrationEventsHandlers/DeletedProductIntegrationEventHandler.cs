using Foundation.EventBus.Abstractions;
using Inventory.Api.IntegrationEvents;
using Inventory.Api.Services.Products;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Inventory.Api.IntegrationEventsHandlers
{
    public class DeletedProductIntegrationEventHandler : IIntegrationEventHandler<DeletedProductIntegrationEvent>
    {
        private readonly IProductService productService;

        public DeletedProductIntegrationEventHandler(
            IProductService productService)
        {
            this.productService = productService;
        }

        /// <summary>
        /// Integration event handler which delete products from inventory when product will deleted
        /// </summary>
        /// <param name="event">
        /// Integration event message which is sent by the
        /// catalog.api once seller deleted product.
        /// </param>
        /// <returns></returns>
        public async Task Handle(DeletedProductIntegrationEvent @event)
        {
            using var source = new ActivitySource(this.GetType().Name);
            using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {@event.GetType().Name}");

            if (@event.OrganisationId.HasValue)
            {
                await this.productService.DeleteProductAsync(@event.ProductId);
            }
        }
    }
}
