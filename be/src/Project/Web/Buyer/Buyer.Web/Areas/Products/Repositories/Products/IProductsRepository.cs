using Buyer.Web.Shared.Brands.DomainModels;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories.Products
{
    public interface IProductsRepository
    {
        Task<Product> GetProductAsync(Guid? productId, string language, string token);
        Task<PagedResults<IEnumerable<Product>>> GetProductsAsync(IEnumerable<Guid> ids, Guid? categoryId, Guid? sellerId, string language, string searchTerm, int pageIndex, int itemsPerPage, string token);
        Task<IEnumerable<string>> GetProductSuggestionsAsync(string searchTerm, int size, string language, string token);
    }
}
