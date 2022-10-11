using System;

namespace Buyer.Web.Areas.Orders.ApiRequestModels
{
    public class UpdateOrderItemStatusApiRequestModel
    {
        public Guid Id { get; set; }
        public Guid OrderItemStatusId { get; set; }
    }
}
