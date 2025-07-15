using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Foundation.Catalog.Repositories.ProductIndexingRepositories
{
    public interface IBulkProductIndexingRepository
    {
        Task IndexAsync(Guid productId);
        Task IndexBatchAsync(IEnumerable<Guid> productIds);
    }
}
