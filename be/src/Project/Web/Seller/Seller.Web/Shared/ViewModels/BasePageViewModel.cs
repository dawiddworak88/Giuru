using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;

namespace Seller.Web.Shared.ViewModels
{
    public class BasePageViewModel
    {
        public string Locale { get; set; }
        public HeaderViewModel Header { get; set; }
        public MenuTilesViewModel MenuTiles { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
