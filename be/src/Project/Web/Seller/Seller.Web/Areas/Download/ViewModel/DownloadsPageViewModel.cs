using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Download.ViewModel
{
    public class DownloadsPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<DomainModels.Download> Catalog { get; set; }
    }
}
