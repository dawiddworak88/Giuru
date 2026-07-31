using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inventory.Api.DomainModels;

namespace Inventory.Api.Repositories.Products
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids, string language = null);
    }
}
