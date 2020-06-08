using Tenant.Portal.Shared.ViewModels;

namespace Tenant.Portal.Areas.Products.ViewModels
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
