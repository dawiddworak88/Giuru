using Buyer.Web.Shared.ViewModels.Base;

namespace Buyer.Web.Areas.Products.ViewModels.Products
{
    public class ProductPageViewModel : BasePageViewModel
    {
        public ProductBreadcrumbsViewModel Breadcrumbs { get; set; }
        public ProductDetailViewModel ProductDetail { get; set; }
    }
}
