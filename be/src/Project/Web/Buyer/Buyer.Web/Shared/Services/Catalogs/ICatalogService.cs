using Buyer.Web.Shared.DomainModels.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Catalogs
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogCategory>> GetCategoriesAsync(string language, int pageIndex, int itemsPerPage);
    }
}
