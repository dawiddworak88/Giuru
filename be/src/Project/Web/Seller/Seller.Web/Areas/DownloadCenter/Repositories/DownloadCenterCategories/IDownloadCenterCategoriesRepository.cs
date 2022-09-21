using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.DownloadCenter.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.DownloadCenter.Repositories.DownloadCenterCategories
{
    public interface IDownloadCenterCategoriesRepository
    {
        Task<PagedResults<IEnumerable<DownloadCenterCategory>>> GetCategoriesAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<IEnumerable<DownloadCenterCategory>> GetCategoriesAsync(string token, string language);
        Task<Guid> SaveAsync(string token, string language, Guid? id, string name, Guid? parentCategoryId, bool isVisible);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<DownloadCenterCategory> GetAsync(string token, string language, Guid? id);
    }
}
