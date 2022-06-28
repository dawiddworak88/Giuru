using System;

namespace Seller.Web.Areas.Orders.ViewModel
{
    public class OrderItemStatusViewModel
    {
        public Guid Id { get; set; }
        public Guid? OrderStatusId { get; set; }
    }
}
