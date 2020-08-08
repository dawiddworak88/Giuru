using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.DomainModels;

namespace Seller.Web.Areas.Products.Repositories
{
    public interface IProductsRepository
    {
        Task<PagedResults<IEnumerable<Product>>> GetProductsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage);
    }
}
