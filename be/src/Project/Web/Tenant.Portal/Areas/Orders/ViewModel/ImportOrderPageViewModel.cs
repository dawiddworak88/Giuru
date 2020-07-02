using Tenant.Portal.Shared.ViewModels;

namespace Tenant.Portal.Areas.Orders.ViewModel
{
    public class ImportOrderPageViewModel : BasePageViewModel
    {
        public string Title { get; set; }
        public ImportOrderFormViewModel ImportOrderForm { get; set; }
    }
}
