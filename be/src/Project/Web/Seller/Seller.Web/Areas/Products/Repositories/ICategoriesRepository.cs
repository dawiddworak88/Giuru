using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Products.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.Repositories
{
    public interface ICategoriesRepository
    {
        Task<PagedResults<IEnumerable<Category>>> GetCategoriesAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage);
        Task<IEnumerable<Category>> GetCategoriesAllAsync(string token, string language, int pageIndex, int itemsPerPage);
        Task DeleteAsync(string token, string language, Guid? id);
    }
}
