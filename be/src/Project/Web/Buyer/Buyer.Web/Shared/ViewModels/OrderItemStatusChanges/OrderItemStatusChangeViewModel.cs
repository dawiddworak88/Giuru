using System;

namespace Buyer.Web.Shared.ViewModels.OrderItemStatusChanges
{
    public class OrderItemStatusChangeViewModel
    {
        public string OrderItemStatusName { get; set; }
        public string OrderItemStatusChangeComment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
