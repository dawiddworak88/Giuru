using System;
using System.Threading.Tasks;

namespace Catalog.BackgroundTasks.Services.Products
{
    public interface IProductsService
    {
        Task IndexAllAsync(Guid? sellerId);
        Task IndexCategoryProducts(Guid? categoryId, Guid? sellerId);
        Task UpdateStockAvailableQuantityAsync(Guid? organisationId, Guid productId, double quantity);
    }
}
