using Foundation.Extensions.Models;
using System;

namespace Ordering.Api.ServicesModels
{
    public class UpdateOrderItemStatusServiceModel : BaseServiceModel
    {
        public Guid? OrderItemId { get; set; }
        public Guid? OrderStatusId { get; set; }
    }
}
