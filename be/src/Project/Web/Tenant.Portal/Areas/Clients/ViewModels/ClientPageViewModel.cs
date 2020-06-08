using Tenant.Portal.Shared.ViewModels;

namespace Tenant.Portal.Areas.Clients.ViewModels
{
    public class ClientPageViewModel : BasePageViewModel
    {
        public string Title { get; set; }
        public string NewText { get; set; }
        public string NewUrl { get; set; }
    }
}
