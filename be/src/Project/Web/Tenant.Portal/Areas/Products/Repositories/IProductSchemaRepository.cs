using System.Threading.Tasks;
using Tenant.Portal.Areas.Products.DomainModels;

namespace Tenant.Portal.Areas.Products.Repositories
{
    public interface IProductSchemaRepository
    {
        Task<Schema> GetProductSchemaAsync(string token, string language);
    }
}
