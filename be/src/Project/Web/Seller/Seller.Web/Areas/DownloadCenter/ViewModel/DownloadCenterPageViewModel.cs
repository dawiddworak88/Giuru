using Seller.Web.Areas.DownloadCenter.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.DownloadCenter.ViewModel
{
    public class DownloadCenterPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<DownloadCenterItem> Catalog { get; set; }
    }
}
