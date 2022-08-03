using Buyer.Web.Shared.ViewModels.Base;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Areas.DownloadCenter.ViewModel
{
    public class DownloadCenterPageViewModel : BasePageViewModel
    {
        public MainNavigationViewModel MainNavigation { get; set; }
        public DownloadCenterCatalogViewModel Catalog { get; set; }
    }
}
