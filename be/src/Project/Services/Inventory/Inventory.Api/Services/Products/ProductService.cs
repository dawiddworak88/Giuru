using Foundation.EventBus.Abstractions;
using Inventory.Api.Infrastructure;
using Inventory.Api.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Inventory.Api.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly InventoryContext _context;
        private readonly IEventBus _eventBus;

        public ProductService(InventoryContext context, IEventBus eventBus)
        {
            _context = context;
            _eventBus = eventBus;
        }

        public async Task DeleteProductAsync(Guid? productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId.Value && x.IsActive);

            if (product is not null)
            {
                product.IsActive = false;
                product.LastModifiedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var upsertedInventoryMessage = new UpsertedInventoryAvailabilityIntegrationEvent
                {
                    ProductSkus = [product.Sku]
                };

                _eventBus.Publish(upsertedInventoryMessage);
            }
        }

        public async Task UpdateProductAsync(Guid? productId, string productName, string productSku, string productEan)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId.Value && x.IsActive);

            if (product is not null)
            {
                var previousSku = product.Sku;

                product.Name = productName;
                product.Sku = productSku;
                product.Ean = productEan;
                product.LastModifiedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                if (previousSku != productSku)
                {
                    var upsertedInventoryMessage = new UpsertedInventoryAvailabilityIntegrationEvent
                    {
                        ProductSkus = [previousSku, productSku]
                    };

                    _eventBus.Publish(upsertedInventoryMessage);
                }
            }
        }
    }
}
