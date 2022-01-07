using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Areas.Products.ViewModels.Brands
{
    public class BrandPageViewModel
    {
        public BuyerHeaderViewModel Header { get; set; }
        public MainNavigationViewModel MainNavigation { get; set; }
        public BrandBreadcrumbsViewModel Breadcrumbs { get; set; }
        public BrandDetailViewModel BrandDetail { get; set; }
        public BrandCatalogViewModel Catalog { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
