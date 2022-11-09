using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Areas.Orders.ViewModel
{
    public class BasketPageViewModel
    {
        public string Locale { get; set; }
        public BuyerHeaderViewModel Header { get; set; }
        public MainNavigationViewModel MainNavigation { get; set; }
        public FooterViewModel Footer { get; set; }
        public BasketFormViewModel BasketForm { get; set; }
    }
}
