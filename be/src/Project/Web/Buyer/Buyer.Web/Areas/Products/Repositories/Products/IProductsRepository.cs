using Buyer.Web.Areas.Products.DomainModels;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories.Products
{
    public interface IProductsRepository
    {
        Task<Product> GetProductAsync(Guid? productId, string language, string token);
        Task<PagedResults<IEnumerable<Product>>> GetProductsAsync(Guid? categoryId, string language, string searchTerm, int pageIndex, int itemsPerPage, string token);
    }
}
