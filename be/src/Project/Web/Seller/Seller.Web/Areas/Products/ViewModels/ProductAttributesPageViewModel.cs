using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ProductAttributesPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<ProductAttribute> Catalog { get; set; }
    }
}
