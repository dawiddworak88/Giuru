using Buyer.Web.Shared.ViewModels.Base;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Areas.DownloadCenter.ViewModel
{
    public class DownloadCenterCategoryPageViewModel : BasePageViewModel
    {
        public MainNavigationViewModel MainNavigation { get; set; }
        public DownloadCenterCategoryBreadcrumbsViewModel Breadcrumbs { get; set; }
        public DownloadCenterCategoryDetailsViewModel CategoryDetails { get; set; }
    }
}
