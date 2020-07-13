using Seller.Portal.Shared.ViewModels;

namespace Seller.Portal.Areas.Orders.ViewModel
{
    public class OrderPageViewModel : BasePageViewModel
    {
        public string Title { get; set; }
        public string NewText { get; set; }
        public string NewUrl { get; set; }
        public string ImportOrderUrl { get; set; }
        public string ImportOrderText { get; set; }
    }
}
