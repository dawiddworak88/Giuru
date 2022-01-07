using Seller.Web.Areas.Inventory.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Inventory.ViewModel
{
    public class WarehousesPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<Warehouse> Catalog { get; set; }
    }
}
