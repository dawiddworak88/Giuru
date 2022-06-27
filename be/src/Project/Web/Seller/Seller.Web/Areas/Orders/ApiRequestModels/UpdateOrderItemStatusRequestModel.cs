using System;

namespace Seller.Web.Areas.Orders.ApiRequestModels
{
    public class UpdateOrderItemStatusRequestModel
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderStatusId { get; set; }
    }
}
