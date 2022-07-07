using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Download.ViewModel
{
    public class DownloadCatalogViewModel
    {
        public string Title { get; set; }
        public string TestUrl { get; set; }
        public PagedResults<IEnumerable<DomainModels.Download>> PagedResults { get; set; }
    }
}
