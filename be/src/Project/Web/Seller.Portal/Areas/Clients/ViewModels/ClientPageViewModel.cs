using Seller.Portal.Shared.ViewModels;

namespace Seller.Portal.Areas.Clients.ViewModels
{
    public class ClientPageViewModel : BasePageViewModel
    {
        public string Title { get; set; }
        public string NewText { get; set; }
        public string NewUrl { get; set; }
    }
}
