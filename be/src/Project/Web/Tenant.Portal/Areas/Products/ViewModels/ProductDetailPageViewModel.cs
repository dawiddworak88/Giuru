using Seller.Portal.Shared.ViewModels;

namespace Seller.Portal.Areas.Products.ViewModels
{
    public class ProductDetailPageViewModel : BasePageViewModel
    {
        public string Title { get; set; }

        public ProductDetailFormViewModel ProductDetailForm { get; set; }
    }
}
