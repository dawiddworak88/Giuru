using System.Threading.Tasks;
using System;

namespace Foundation.Catalog.Repositories.ProductIndexingRepositories
{
    public interface IBulkProductIndexingRepository
    {
        Task IndexAsync(Guid productId);
    }
}
