using Buyer.Web.Shared.Catalogs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Catalogs.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync(string language);
    }
}
