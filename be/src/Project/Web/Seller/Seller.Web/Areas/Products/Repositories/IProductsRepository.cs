using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.DomainModels;
using System;

namespace Seller.Web.Areas.Products.Repositories
{
    public interface IProductsRepository
    {
        Task DeleteAsync(string token, string language, Guid? id);
        Task<Product> GetProductAsync(string token, string language, Guid? id);
        Task<PagedResults<IEnumerable<Product>>> GetProductsAsync(string token, string language, string searchTerm, Guid? sellerId, int pageIndex, int itemsPerPage);
        Task<IEnumerable<Product>> GetAllPrimaryProductsAsync(string token, string language, Guid? sellerId, int pageIndex, int itemsPerPage);
    }
}
