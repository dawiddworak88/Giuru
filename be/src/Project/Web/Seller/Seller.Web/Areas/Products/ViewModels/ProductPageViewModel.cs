using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ProductPageViewModel : BasePageViewModel
    {
        public string Title { get; set; }

        public ProductDetailFormViewModel ProductDetailForm { get; set; }
    }
}
