using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Feature.Order.Models
{
    public class OrderValidationModel : BaseServiceModel
    {
        public Guid? ClientId { get; set; }
        public IEnumerable<OrderItemModel> OrderItems { get; set; }
    }
}
