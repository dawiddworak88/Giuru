using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Areas.Products.ViewModels.AvailableProducts
{
    public class AvailableProductsPageViewModel
    {
        public BuyerHeaderViewModel Header { get; set; }
        public MainNavigationViewModel MainNavigation { get; set; }
        public AvailableProductsCatalogViewModel Catalog { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
