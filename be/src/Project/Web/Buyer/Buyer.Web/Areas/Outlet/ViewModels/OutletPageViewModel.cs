using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Areas.Outlet.ViewModels
{
    public class OutletPageViewModel
    {
        public BuyerHeaderViewModel Header { get; set; }
        public MainNavigationViewModel MainNavigation { get; set; }
        public OutletPageCatalogViewModel Catalog { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
