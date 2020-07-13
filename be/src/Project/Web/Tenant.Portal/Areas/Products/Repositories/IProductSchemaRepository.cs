using System;
using System.Threading.Tasks;
using Seller.Portal.Areas.Products.DomainModels;

namespace Seller.Portal.Areas.Products.Repositories
{
    public interface IProductSchemaRepository
    {
        Task<Schema> GetProductSchemaByIdAsync(string token, string language, Guid? id);
        Task<Schema> GetProductSchemaByEntityTypeIdAsync(string token, string language, Guid? id);
    }
}
