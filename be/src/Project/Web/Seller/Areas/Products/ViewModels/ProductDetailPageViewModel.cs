using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ProductDetailPageViewModel : BasePageViewModel
    {
        public string Title { get; set; }

        public ProductDetailFormViewModel ProductDetailForm { get; set; }
    }
}
