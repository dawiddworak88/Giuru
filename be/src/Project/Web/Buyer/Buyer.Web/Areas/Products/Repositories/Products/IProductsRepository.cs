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
        Task<Product> GetProductAsync(string sku, string language, string token);
        Task<PagedResults<IEnumerable<Product>>> GetProductsAsync(string token, string language, string searchTerm, bool? hasPrimaryProduct, Guid? sellerId, int pageIndex, int itemsPerPage, string orderBy);
        Task<PagedResults<IEnumerable<Product>>> GetProductsAsync(IEnumerable<Guid> ids, Guid? categoryId, Guid? sellerId, string language, string searchTerm, bool? hasPrimaryProduct, int pageIndex, int itemsPerPage, string token, string orderBy);
        Task<ProductStock> GetProductStockAsync(Guid? productId);
        Task<ProductStock> GetProductOutletAsync(Guid? productId);
        Task<IEnumerable<string>> GetProductSuggestionsAsync(string searchTerm, int size, string language, string token);
        Task<PagedResults<IEnumerable<ProductFile>>> GetProductFilesAsync(string token, string language, Guid? id, int pageIndex, int itemsPerPage, string searchTerm, string orderBy);
        Task<IEnumerable<Product>> GetProductsBySkusAsync(string token, string language, IEnumerable<string> skus);
    }
}
