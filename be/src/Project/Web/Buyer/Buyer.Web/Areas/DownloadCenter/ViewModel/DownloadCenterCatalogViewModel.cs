using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;

namespace Buyer.Web.Areas.DownloadCenter.ViewModel
{
    public class DownloadCenterCatalogViewModel
    {
        public string Title { get; set; }
        public string TestUrl { get; set; }
        public PagedResults<IEnumerable<DomainModels.DownloadCenterItem>> PagedResults { get; set; }
    }
}
