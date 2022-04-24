using System;
using System.Threading.Tasks;

namespace Inventory.Api.Services.Products
{
    public interface IProductService
    {
        Task UpdateProductAsync(Guid? productId, string productName, string productSku, string productEan);
    }
}
