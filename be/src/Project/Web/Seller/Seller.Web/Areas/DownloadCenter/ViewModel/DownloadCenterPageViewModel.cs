using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.DownloadCenter.ViewModel
{
    public class DownloadCenterPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<DomainModels.DownloadCenter> Catalog { get; set; }
    }
}
