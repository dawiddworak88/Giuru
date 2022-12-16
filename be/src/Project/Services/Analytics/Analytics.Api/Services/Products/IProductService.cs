using System.Threading.Tasks;
using System;

namespace Analytics.Api.Services.Products
{
    public interface IProductService
    {
        Task UpdateProductAsync(Guid? productId, string productName, string productSku, string productEan, string language);
    }
}
