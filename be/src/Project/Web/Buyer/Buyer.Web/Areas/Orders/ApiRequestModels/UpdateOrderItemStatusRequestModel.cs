using System;

namespace Buyer.Web.Areas.Orders.ApiRequestModels
{
    public class UpdateOrderItemStatusRequestModel
    {
        public Guid Id { get; set; }
        public Guid OrderItemStatusId { get; set; }
    }
}
