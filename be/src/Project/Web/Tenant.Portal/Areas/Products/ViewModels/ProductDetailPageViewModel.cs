using Tenant.Portal.Shared.ViewModels;

namespace Tenant.Portal.Areas.Products.ViewModels
{
    public class ProductDetailPageViewModel : BasePageViewModel
    {
        public string Title { get; set; }

        public ProductDetailFormViewModel ProductDetailForm { get; set; }
    }
}
