using Seller.Web.Areas.Inventory.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Inventory.ViewModel
{
    public class OutletsPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<OutletItem> Catalog { get; set; }
    }
}
