using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Buyer.Web.Shared.DomainModels.Inventory;

namespace Buyer.Web.Shared.Repositories.Inventory
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<InventoryItem>> GetStockAvailbleProductsByProductIdsAsync(string token, string language, IEnumerable<Guid> ids);
    }
}
