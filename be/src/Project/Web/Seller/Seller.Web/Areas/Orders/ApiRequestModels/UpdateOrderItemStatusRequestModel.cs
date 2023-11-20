using System;

namespace Seller.Web.Areas.Orders.ApiRequestModels
{
    public class UpdateOrderItemStatusRequestModel
    {
        public Guid Id { get; set; }
        public Guid OrderItemStatusId { get; set; }
        public string ExpectedDateOfProductOnStock { get; set; }
    }
}
