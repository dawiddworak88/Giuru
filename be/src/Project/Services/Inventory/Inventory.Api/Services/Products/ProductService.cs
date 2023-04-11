using Foundation.Extensions.ExtensionMethods;
using Inventory.Api.Infrastructure;
using Inventory.Api.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task UpdateProductAsync(Guid? productId, string productName, string productSku, string productEan, IEnumerable<Guid> clientGroupIds)
        {
            var #product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId.Value && x.IsActive);

            if (product is not null)
            {
                product.Name = productName;
                product.Sku = productSku;
                product.Ean = productEan;
                product.LastModifiedDate = DateTime.UtcNow;

                var clientGroups = _context.ProductsGroups.Where(x => x.ProductId == productId && x.IsActive);

                foreach (var clientGroup in clientGroups.OrEmptyIfNull())
                {
                    _context.ProductsGroups.Remove(clientGroup);
                }

                foreach (var clientGroupId in clientGroupIds.OrEmptyIfNull())
                {
                    var group = new ProductsGroup
                    {
                        ProductId = product.Id,
                        GroupId = clientGroupId
                    };

                    await _context.ProductsGroups.AddAsync(group);
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
