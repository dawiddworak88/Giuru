using Buyer.Web.Areas.Download.DomainModels;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Download.Repositories
{
    public interface IDownloadsRepository
    {
        Task<PagedResults<IEnumerable<DomainModels.Download>>> GetAsync(string token, string language, int pageIndex, int itemsPerPage, string searchTerm, string orderBy);
        Task<DownloadCategory2> GetAsync(string token, string language, Guid? id);
    }
}
