using Tenant.Portal.Shared.Footers.ViewModels;
using Tenant.Portal.Shared.Headers.ViewModels;
using Tenant.Portal.Shared.MenuTiles.ViewModels;

namespace Tenant.Portal.Areas.Orders.ViewModel
{
    public class OrderPageViewModel
    {
        public HeaderViewModel Header { get; set; }
        public MenuTilesViewModel MenuTiles { get; set; }
        public FooterViewModel Footer { get; set; }
        public string Welcome { get; set; }
        public string LearnMore { get; set; }
    }
}
