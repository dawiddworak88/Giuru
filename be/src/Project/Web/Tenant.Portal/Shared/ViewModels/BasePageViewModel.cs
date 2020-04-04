using Feature.PageContent.Components.Footers.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.MenuTiles.ViewModels;

namespace Tenant.Portal.Shared.ViewModels
{
    public class BasePageViewModel
    {
        public HeaderViewModel Header { get; set; }
        public MenuTilesViewModel MenuTiles { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
