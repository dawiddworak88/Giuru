using Buyer.Web.Shared.ViewModels.Base;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Areas.Products.ViewModels.Categories
{
    public class CategoryPageViewModel : BasePageViewModel
    {
        public MainNavigationViewModel MainNavigation { get; set; }
        public CategoryBreadcrumbsViewModel Breadcrumbs { get; set; }
        public CategoryCatalogViewModel Catalog { get; set; }
    }
}
