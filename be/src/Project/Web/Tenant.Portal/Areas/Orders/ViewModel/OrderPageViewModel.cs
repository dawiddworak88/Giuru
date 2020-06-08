using Tenant.Portal.Shared.ViewModels;

namespace Tenant.Portal.Areas.Orders.ViewModel
{
    public class OrderPageViewModel : BasePageViewModel
    {
        public string Title { get; set; }
        public string NewText { get; set; }
        public string NewUrl { get; set; }
    }
}
