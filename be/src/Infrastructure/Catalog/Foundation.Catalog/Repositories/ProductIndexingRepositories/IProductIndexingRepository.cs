using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foundation.Catalog.Repositories.ProductIndexingRepositories
{
    public interface IProductIndexingRepository
    {
        Task IndexAsync(Guid productId);
        Task DeleteAsync(Guid sellerId);
        Task BulkUpdateStockAvailableQuantity(IEnumerable<(string docId, double availableQuantity)> updates);
        Task BulkUpdateOutletAvailableQuantity(IEnumerable<(string docId, double availableQuantity)> updates);
    }
}
