using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Areas.DownloadCenter.ViewModel
{
    public class DownloadCenterCategoryPageViewModel
    {
        public string Locale { get; set; }
        public BuyerHeaderViewModel Header { get; set; }
        public MainNavigationViewModel MainNavigation { get; set; }
        public DownloadCenterCategoryBreadcrumbsViewModel Breadcrumbs { get; set; }
        public DownloadCenterCategoryDetailsViewModel CategoryDetails { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
