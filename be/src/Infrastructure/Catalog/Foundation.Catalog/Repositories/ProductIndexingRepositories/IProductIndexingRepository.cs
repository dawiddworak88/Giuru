using System;
using System.Threading.Tasks;

namespace Foundation.Catalog.Repositories.Products.ProductIndexingRepositories
{
    public interface IProductIndexingRepository
    {
        Task IndexAsync(Guid productId);
        Task DeleteAsync(Guid sellerId);
    }
}
