using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Orders.ViewModel
{
    public class OrdersPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<Order> Catalog { get; set; }
    }
}
