using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Areas.Products.ViewModels.Products
{
    public class ProductPageViewModel
    {
        public BuyerHeaderViewModel Header { get; set; }
        public MainNavigationViewModel MainNavigation { get; set; }
        public ProductBreadcrumbsViewModel Breadcrumbs { get; set; }
        public ProductDetailViewModel ProductDetail { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
