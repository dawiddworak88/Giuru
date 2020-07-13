using Seller.Portal.Shared.ViewModels;

namespace Seller.Portal.Areas.Clients.ViewModels
{
    public class ClientDetailPageViewModel : BasePageViewModel
    {
        public string Title { get; set; }

        public ClientDetailFormViewModel ClientDetailForm { get; set; }
    }
}
