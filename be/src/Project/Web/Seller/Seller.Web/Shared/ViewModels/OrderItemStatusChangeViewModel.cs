using System;

namespace Seller.Web.Shared.ViewModels
{
    public class OrderItemStatusChangeViewModel
    {
        public string OrderItemStatusName { get; set; }
        public string OrderItemStatusChangeComment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
