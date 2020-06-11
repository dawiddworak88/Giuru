using System.Threading.Tasks;

namespace Tenant.Portal.Areas.Products.Repositories
{
    public interface IProductSchemaRepository
    {
        Task<string> GetProductSchemaAsync(string token, string language);
    }
}
