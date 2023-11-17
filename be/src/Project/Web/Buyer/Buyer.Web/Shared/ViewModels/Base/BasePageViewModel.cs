using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.PageContent.Components.Footers.ViewModels;
using Buyer.Web.Shared.ViewModels.NotificationBar;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Shared.ViewModels.Base
{
    public class BasePageViewModel
    {
        public string Locale { get; set; }
        public NotificationBarViewModel NotificationBar { get; set; }
        public BuyerHeaderViewModel Header { get; set; }
        public MainNavigationViewModel MainNavigation { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
