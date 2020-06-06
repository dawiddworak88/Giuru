using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tenant.Portal.Areas.Products.DomainModels;

namespace Tenant.Portal.Areas.Products.Repositories
{
    public interface IProductsRepository
    {
        Task<PagedResults<IEnumerable<Product>>> GetProductsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage);
    }
}
