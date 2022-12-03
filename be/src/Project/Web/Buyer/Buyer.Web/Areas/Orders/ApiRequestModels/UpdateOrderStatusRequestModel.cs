using System;

namespace Buyer.Web.Areas.Orders.ApiRequestModels
{
    public class UpdateOrderStatusRequestModel
    {
        public Guid OrderId { get; set; }
        public Guid OrderStatusId { get; set; }
    }
}
