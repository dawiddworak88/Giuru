using Buyer.Web.Shared.ViewModels.Base;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Areas.News.ViewModel
{
    public class NewsItemPageViewModel : BasePageViewModel
    {
        public MainNavigationViewModel MainNavigation { get; set; }
        public NewsItemBreadcrumbsViewModel Breadcrumbs { get; set; }
        public NewsItemDetailsViewModel NewsItemDetails { get; set; }
    }
}
