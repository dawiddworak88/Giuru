using System;
using System.Threading.Tasks;

namespace Catalog.Api.Repositories.Products.ProductIndexingRepositories
{
    public interface IProductIndexingRepository
    {
        Task IndexAsync(Guid productId);
    }
}
