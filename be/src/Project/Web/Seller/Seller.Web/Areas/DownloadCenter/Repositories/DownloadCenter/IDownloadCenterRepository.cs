using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.DownloadCenter.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.DownloadCenter.Repositories.DownloadCenter
{
    public interface IDownloadCenterRepository
    {
        Task<PagedResults<IEnumerable<DownloadCenterItem>>> GetDownloadCenterItemsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<DownloadCenterCategoryFile> GetAsync(string token, string language, Guid? id);
        Task<Guid> SaveAsync(string token, string language, Guid? id, IEnumerable<Guid> categoriesIds, IEnumerable<DownloadCenterApiFile> files);
        Task DeleteAsync(string token, string language, Guid? id);
    }
}
