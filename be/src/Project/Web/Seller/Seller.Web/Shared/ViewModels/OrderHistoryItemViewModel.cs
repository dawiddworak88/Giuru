using System;

namespace Seller.Web.Shared.ViewModels
{
    public class OrderHistoryItemViewModel
    {
        public string OrderStatusName { get; set; }
        public string OrderStatusComment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
