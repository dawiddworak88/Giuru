using Tenant.Portal.Shared.ViewModels;

namespace Tenant.Portal.Areas.Clients.ViewModels
{
    public class ClientDetailPageViewModel : BasePageViewModel
    {
        public string Title { get; set; }

        public ClientDetailFormViewModel ClientDetailForm { get; set; }
    }
}
