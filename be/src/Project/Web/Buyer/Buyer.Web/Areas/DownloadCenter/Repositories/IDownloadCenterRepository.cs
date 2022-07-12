using Buyer.Web.Areas.DownloadCenter.DomainModels;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.DownloadCenter.Repositories
{
    public interface IDownloadCenterRepository
    {
        Task<PagedResults<IEnumerable<DomainModels.Download>>> GetAsync(string token, string language, int pageIndex, int itemsPerPage, string searchTerm, string orderBy);
        Task<DownloadCategory2> GetAsync(string token, string language, Guid? id);
    }
}
