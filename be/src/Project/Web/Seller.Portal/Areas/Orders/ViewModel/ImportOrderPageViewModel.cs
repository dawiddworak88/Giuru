using Seller.Portal.Shared.ViewModels;

namespace Seller.Portal.Areas.Orders.ViewModel
{
    public class ImportOrderPageViewModel : BasePageViewModel
    {
        public string Title { get; set; }
        public ImportOrderFormViewModel ImportOrderForm { get; set; }
    }
}
