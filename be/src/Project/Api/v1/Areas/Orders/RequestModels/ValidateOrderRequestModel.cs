using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Api.v1.Areas.Orders.RequestModels
{
    public class ValidateOrderRequestModel : BaseRequestModel
    {
        public Guid ClientId { get; set; }
        public IEnumerable<OrderItemRequestModel> OrderItems { get; set; }
    }
}
