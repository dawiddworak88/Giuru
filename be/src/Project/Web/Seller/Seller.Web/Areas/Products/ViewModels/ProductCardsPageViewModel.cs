using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ProductCardsPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<ProductCardCategory> Catalog { get; set; }
    }
}
