using Buyer.Web.Shared.ViewModels.Base;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Areas.Dashboard.ViewModels
{
    public class DashboardPageViewModel
    {

        public string Locale { get; set; }
        public BuyerHeaderViewModel Header { get; set; }
        public MainNavigationViewModel MainNavigation { get; set; }
        public OrdersAnalyticsDetailViewModel OrdersAnalyticsDetail { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
