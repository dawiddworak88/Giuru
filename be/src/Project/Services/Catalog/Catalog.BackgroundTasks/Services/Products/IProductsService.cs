using Catalog.BackgroundTasks.ServicesModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.BackgroundTasks.Services.Products
{
    public interface IProductsService
    {
        Task IndexAllAsync(Guid? sellerId);
        Task IndexCategoryProducts(Guid? categoryId, Guid? sellerId);
        Task BatchUpdateStockAvailableQuantitiesAsync(IEnumerable<AvailableQuantityServiceModel> availableQuantities);
        Task BatchUpdateOutletAvailableQuantitiesAsync(IEnumerable<AvailableQuantityServiceModel> availableQuantities);
    }
}
