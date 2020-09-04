using Buyer.Web.Shared.Catalogs.ViewModels;
using Buyer.Web.Shared.Headers.ViewModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Areas.Products.ViewModels.Categories
{
    public class CategoryPageViewModel
    {
        public BuyerHeaderViewModel Header { get; set; }
        public MainNavigationViewModel MainNavigation { get; set; }
        public CatalogViewModel Catalog { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
