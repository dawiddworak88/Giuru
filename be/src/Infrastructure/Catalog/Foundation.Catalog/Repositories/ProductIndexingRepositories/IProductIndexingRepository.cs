using System;
using System.Threading.Tasks;

namespace Foundation.Catalog.Repositories.ProductIndexingRepositories
{
    public interface IProductIndexingRepository
    {
        Task IndexAsync(Guid productId);
        Task DeleteAsync(Guid sellerId);
    }
}
