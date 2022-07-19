using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.DownloadCenter.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.DownloadCenter.Repositories.DownloadCenter
{
    public interface IDownloadCenterRepository
    {
        Task<PagedResults<IEnumerable<DownloadCenterItem>>> GetDownloadCenterAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<DownloadCenterFile> GetAsync(string token, string language, Guid? id);
        Task<Guid> SaveAsync(string token, string language, Guid? id, IEnumerable<Guid> categoriesIds, IEnumerable<Guid> files);
        Task DeleteAsync(string token, string language, Guid? id);
    }
}
