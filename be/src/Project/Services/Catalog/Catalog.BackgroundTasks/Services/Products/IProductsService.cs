using System;
using System.Threading.Tasks;

namespace Catalog.BackgroundTasks.Services.Products
{
    public interface IProductsService
    {
        Task IndexAllAsync(Guid? sellerId);
    }
}
