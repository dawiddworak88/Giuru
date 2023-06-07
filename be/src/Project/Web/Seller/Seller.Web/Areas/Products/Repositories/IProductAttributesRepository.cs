using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Products.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.Repositories
{
    public interface IProductAttributesRepository
    {
        Task<Guid> SaveAsync(string token, string language, Guid? id, string name);
        Task<PagedResults<IEnumerable<ProductAttribute>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<ProductAttribute> GetByIdAsync(string token, string language, Guid? id);
        Task<IEnumerable<ProductAttribute>> GetAsync(string token, string language);
    }
}
