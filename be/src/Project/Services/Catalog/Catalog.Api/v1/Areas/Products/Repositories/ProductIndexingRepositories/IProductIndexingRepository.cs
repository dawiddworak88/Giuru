using System;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Products.Repositories.ProductIndexingRepositories
{
    public interface IProductIndexingRepository
    {
        Task IndexAsync(Guid productId);
    }
}
