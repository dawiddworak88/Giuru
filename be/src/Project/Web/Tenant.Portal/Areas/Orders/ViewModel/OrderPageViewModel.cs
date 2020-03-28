using Feature.PageContent.MenuTiles.ViewModels;
using Feature.PageContent.Shared.Footers.ViewModels;
using Feature.PageContent.Shared.Headers.ViewModels;

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
