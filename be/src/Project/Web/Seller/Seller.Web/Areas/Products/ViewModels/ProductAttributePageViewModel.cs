using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ProductAttributePageViewModel : BasePageViewModel
    {
        public CatalogViewModel<ProductAttributeItem> Catalog { get; set; }
    }
}
