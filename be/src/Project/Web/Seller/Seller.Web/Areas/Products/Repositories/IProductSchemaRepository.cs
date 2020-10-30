using System;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.DomainModels;

namespace Seller.Web.Areas.Products.Repositories
{
    public interface IProductSchemaRepository
    {
        Task<Schema> GetProductSchemaByIdAsync(string token, string language, Guid? id);
        Task<Schema> GetProductSchemaByEntityTypeIdAsync(string token, string language, Guid? id);
    }
}
