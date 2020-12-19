using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ProductsPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<Product> Catalog { get; set; }
    }
}
