using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Areas.Orders.ViewModel
{
    public class OrdersPageViewModel
    {
        public string Locale { get; set; }
        public BuyerHeaderViewModel Header { get; set; }
        public MainNavigationViewModel MainNavigation { get; set; }
        public FooterViewModel Footer { get; set; }
        public CatalogOrderViewModel<Order> Catalog { get; set; }
    }
}
