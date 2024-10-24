﻿using Foundation.EventBus.Abstractions;
using Inventory.Api.IntegrationEvents;
using Inventory.Api.Services.Products;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Inventory.Api.IntegrationEventsHandlers
{
    public class UpdatedProductIntegrationEventHandler : IIntegrationEventHandler<UpdatedProductIntegrationEvent>
    {
        private readonly IProductService productService;

        public UpdatedProductIntegrationEventHandler(IProductService productService)
        {
            this.productService = productService;
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
            using var source = new ActivitySource(this.GetType().Name);
            using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {@event.GetType().Name}");

            if (@event.OrganisationId.HasValue)
            {
                await this.productService.UpdateProductAsync(@event.ProductId, @event.ProductName, @event.ProductSku, @event.ProductEan);
            }
        }
    }
}
