using Seller.Web.Areas.DownloadCenter.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.DownloadCenter.ViewModel
{
    public class DownloadCenterCategoriesPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<DownloadCenterCategory> Catalog { get; set; }
    }
}
