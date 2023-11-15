using Buyer.Web.Shared.ViewModels.Base;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Areas.Products.ViewModels.Products
{
    public class ProductPageViewModel : BasePageViewModel
    {
        public MainNavigationViewModel MainNavigation { get; set; }
        public ProductBreadcrumbsViewModel Breadcrumbs { get; set; }
        public ProductDetailViewModel ProductDetail { get; set; }
    }
}
