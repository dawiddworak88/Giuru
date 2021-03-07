using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Products.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.Repositories
{
    public interface IProductAttributesRepository
    {
        Task<Guid> SaveAsync(string token, string language, Guid? id, string key, string name);
        Task<PagedResults<IEnumerable<ProductAttribute>>> GetProductAttributesAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<ProductAttribute> GetProductAttributeAsync(string token, string language, Guid? id);
        Task<PagedResults<IEnumerable<ProductAttributeItem>>> GetProductAttributeItemsAsync(string token, string language, Guid? productAttributeId, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
    }
}
