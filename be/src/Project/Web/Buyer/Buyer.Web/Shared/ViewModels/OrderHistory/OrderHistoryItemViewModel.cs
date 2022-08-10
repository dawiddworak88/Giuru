using System;

namespace Buyer.Web.Shared.ViewModels.OrderHistory
{
    public class OrderHistoryItemViewModel
    {
        public string OrderStatusName { get; set; }
        public string OrderStatusComment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
