using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Api.Services.Products
{
    public interface IProductService
    {
        Task UpdateProductAsync(Guid? productId, string productName, string productSku, string productEan, IEnumerable<Guid> groupIds);
        Task DeleteProductAsync(Guid? productId);
    }
}
