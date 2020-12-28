using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Orders.ViewModel
{
    public class OrdersPageViewModel : BasePageViewModel
    {
        public string Title { get; set; }
        public string NewText { get; set; }
        public string NewUrl { get; set; }
        public string ImportOrderUrl { get; set; }
        public string ImportOrderText { get; set; }
    }
}
