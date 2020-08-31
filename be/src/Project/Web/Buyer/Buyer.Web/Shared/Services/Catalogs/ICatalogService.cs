using Buyer.Web.Shared.Models.Catalogs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Catalogs
{
    public interface ICatalogService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync(string language);
    }
}
