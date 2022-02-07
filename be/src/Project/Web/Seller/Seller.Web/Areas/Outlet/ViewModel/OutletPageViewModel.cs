using Seller.Web.Areas.Outlet.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Outlet.ViewModel
{
    public class OutletPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<OutletItem> Catalog { get; set; }
    }
}
