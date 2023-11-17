using Buyer.Web.Shared.ViewModels.Base;

namespace Buyer.Web.Areas.Products.ViewModels.Categories
{
    public class CategoryPageViewModel : BasePageViewModel
    {
        public CategoryBreadcrumbsViewModel Breadcrumbs { get; set; }
        public CategoryCatalogViewModel Catalog { get; set; }
    }
}
