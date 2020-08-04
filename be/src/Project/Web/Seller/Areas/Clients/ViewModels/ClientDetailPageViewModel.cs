using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientDetailPageViewModel : BasePageViewModel
    {
        public string Title { get; set; }

        public ClientDetailFormViewModel ClientDetailForm { get; set; }
    }
}
