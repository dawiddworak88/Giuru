using Buyer.Web.Areas.DownloadCenter.DomainModels;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.DownloadCenter.Repositories
{
    public interface IDownloadCenterRepository
    {
        Task<PagedResults<IEnumerable<DownloadCenterItem>>> GetAsync(string token, string language, int pageIndex, int itemsPerPage, string searchTerm, string orderBy);
        Task<DownloadCenterCategory> GetAsync(string token, string language, Guid? id);
        Task<PagedResults<IEnumerable<DownloadCenterFile>>> GetCategoryFilesAsync(string token, string language, Guid? id, int pageIndex, int itemsPerPage, string searchTerm, string orderBy);
    }
}
