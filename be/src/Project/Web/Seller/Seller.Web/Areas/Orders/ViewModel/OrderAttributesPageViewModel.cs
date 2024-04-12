using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Orders.ViewModel
{
    public class OrderAttributesPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<OrderAttribute> Catalog { get; set; }
    }
}
