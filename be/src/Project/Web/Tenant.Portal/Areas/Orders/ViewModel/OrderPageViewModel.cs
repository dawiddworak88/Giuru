using Feature.PageContent.MenuTiles.ViewModels;
using Feature.PageContent.Components.Footers.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;

namespace Tenant.Portal.Areas.Orders.ViewModel
{
    public class OrderPageViewModel
    {
        public HeaderViewModel Header { get; set; }
        public MenuTilesViewModel MenuTiles { get; set; }
        public OrderCatalogViewModel Catalog { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
