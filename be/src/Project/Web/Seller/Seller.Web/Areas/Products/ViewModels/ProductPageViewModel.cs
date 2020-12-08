using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ProductPageViewModel : BasePageViewModel
    {
        public string Title { get; set; }
        public string NewText { get; set; }
        public string NewUrl { get; set; }
        public CatalogViewModel<Product> Catalog { get; set; }
    }
}
