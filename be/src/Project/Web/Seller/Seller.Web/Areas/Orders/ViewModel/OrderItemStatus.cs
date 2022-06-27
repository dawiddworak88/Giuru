using System;

namespace Seller.Web.Areas.Orders.ViewModel
{
    public class OrderItemStatus
    {
        public Guid Id { get; set; }
        public Guid? OrderStatusId { get; set; }
    }
}
