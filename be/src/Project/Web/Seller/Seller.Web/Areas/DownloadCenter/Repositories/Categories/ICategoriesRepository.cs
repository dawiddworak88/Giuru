using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.DownloadCenter.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.DownloadCenter.Repositories.Categories
{
    public interface ICategoriesRepository
    {
        Task<PagedResults<IEnumerable<Category>>> GetCategoriesAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<IEnumerable<Category>> GetCategoriesAsync(string token, string language);
        Task<Guid> SaveAsync(string token, string language, Guid? id, string name, Guid? parentCategoryId, IEnumerable<Guid> files);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<Category> GetAsync(string token, string language, Guid? id);
    }
}
