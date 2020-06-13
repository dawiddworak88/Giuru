using System;
using System.Threading.Tasks;
using Tenant.Portal.Areas.Products.DomainModels;

namespace Tenant.Portal.Areas.Products.Repositories
{
    public interface IProductSchemaRepository
    {
        Task<Schema> GetProductSchemaByIdAsync(string token, string language, Guid? id);
        Task<Schema> GetProductSchemaByEntityTypeIdAsync(string token, string language, Guid? id);
    }
}
