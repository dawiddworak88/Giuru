using Feature.PageContent.Components.Footers.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.MenuTiles.ViewModels;

namespace Tenant.Portal.Areas.Clients.ViewModels
{
    public class ClientPageViewModel
    {
        public HeaderViewModel Header { get; set; }
        public MenuTilesViewModel MenuTiles { get; set; }
        public ClientCatalogViewModel Catalog { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
