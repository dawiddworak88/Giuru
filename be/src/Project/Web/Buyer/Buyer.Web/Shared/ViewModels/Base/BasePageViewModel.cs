using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.NotificationBar.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;

namespace Buyer.Web.Shared.ViewModels.Base
{
    public class BasePageViewModel
    {
        public string Locale { get; set; }
        public NotificationBarViewModel NotificationBar { get; set; }
        public HeaderViewModel Header { get; set; }
        public MenuTilesViewModel MenuTiles { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
