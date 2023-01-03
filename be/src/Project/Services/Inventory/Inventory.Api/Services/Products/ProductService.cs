using Inventory.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Inventory.Api.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly InventoryContext _context;

        public ProductService(InventoryContext context)
        {
            _context = context;
        }

        public async Task DeleteProductAsync(Guid? productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId.Value && x.IsActive);

            if (product is not null)
            {
                product.IsActive = false;
                product.LastModifiedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateProductAsync(Guid? productId, string productName, string productSku, string productEan)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId.Value && x.IsActive);

            if (product is not null)
            {
                product.Name = productName;
                product.Sku = productSku;
                product.Ean = productEan;
                product.LastModifiedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }
    }
}
