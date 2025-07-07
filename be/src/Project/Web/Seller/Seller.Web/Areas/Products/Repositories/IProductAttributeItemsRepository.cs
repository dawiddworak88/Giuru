using Foundation.GenericRepository.Paginations;
using Seller.Web.Shared.DomainModels.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.Repositories
{
    public interface IProductAttributeItemsRepository
    {
        Task<Guid> SaveAsync(string token, string language, Guid? id, Guid? productAttributeId, string name);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<ProductAttributeItem> GetByIdAsync(string token, string language, Guid? id);
        Task<PagedResults<IEnumerable<ProductAttributeItem>>> GetAsync(string token, string language, Guid? productAttributeId, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<IEnumerable<ProductAttributeItem>> GetAsync(string token, string language, Guid? productAttributeId);
    }
}
