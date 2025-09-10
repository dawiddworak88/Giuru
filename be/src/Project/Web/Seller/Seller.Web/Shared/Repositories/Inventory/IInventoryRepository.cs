using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Seller.Web.Shared.DomainModels.Inventory;

namespace Seller.Web.Shared.Repositories.Inventory
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<InventoryItem>> GetAvailbleProductsByProductIdsAsync(string token, string language, IEnumerable<Guid> ids);
    }
}
