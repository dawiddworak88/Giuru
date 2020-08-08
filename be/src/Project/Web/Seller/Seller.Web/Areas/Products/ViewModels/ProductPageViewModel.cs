using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ProductPageViewModel : BasePageViewModel
    {
        public string Locale { get; set; }
        public string Title { get; set; }
        public string NewText { get; set; }
        public string NewUrl { get; set; }
        public ProductPageCatalogViewModel Catalog { get; set; }
    }
}
