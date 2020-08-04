using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Orders.ViewModel
{
    public class ImportOrderPageViewModel : BasePageViewModel
    {
        public string Title { get; set; }
        public ImportOrderFormViewModel ImportOrderForm { get; set; }
    }
}
