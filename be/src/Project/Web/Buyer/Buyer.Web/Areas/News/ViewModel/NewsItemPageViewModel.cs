using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Areas.News.ViewModel
{
    public class NewsItemPageViewModel
    {
        public BuyerHeaderViewModel Header { get; set; }
        public MainNavigationViewModel MainNavigation { get; set; }
        public NewsItemDetailsViewModel NewsItemDetails { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
