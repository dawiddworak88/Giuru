using System;

namespace Seller.Web.Areas.Orders.ApiRequestModels
{
    public class UpdateOrderItemStatusRequestModel
    {
        public Guid Id { get; set; }
        public Guid OrderStatusId { get; set; }
        public string OrderStatusComment { get; set; }
    }
}
