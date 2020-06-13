using System;
using System.Threading.Tasks;
using Tenant.Portal.Areas.Products.DomainModels;

namespace Tenant.Portal.Areas.Products.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetProductAsync(string token, string language, Guid? id);
    }
}
