using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.DomainModels;
using System;

namespace Seller.Web.Areas.Shared.Repositories.Products
{
    public interface IProductsRepository
    {
        Task DeleteAsync(string token, string language, Guid? id);
        Task<Product> GetProductAsync(string token, string language, Guid? id);
        Task<Product> GetProductAsync(string token, string language, string sku);
        Task<PagedResults<IEnumerable<Product>>> GetProductsAsync(string token, string language, string searchTerm, bool? hasPrimaryProduct, Guid? sellerId, int pageIndex, int itemsPerPage, string orderBy);
        Task<IEnumerable<Product>> GetAllProductsAsync(string token, string language, IEnumerable<Guid> productIds);
        Task<IEnumerable<Product>> GetAllPrimaryProductsAsync(string token, string language, Guid? sellerId, string orderBy);
        Task<Guid> SaveAsync(
            string token, 
            string language, 
            Guid? id, 
            string name, 
            string sku, 
            string description, 
            bool isNew,
            bool isPublished,
            Guid? primaryProductId, 
            Guid? categoryId, 
            IEnumerable<Guid> images,
            IEnumerable<Guid> files,
            string ean,
            int fulfillmentTime,
            string formData);
        Task TriggerProductsReindexingAsync(string token, string language);
    }
}
