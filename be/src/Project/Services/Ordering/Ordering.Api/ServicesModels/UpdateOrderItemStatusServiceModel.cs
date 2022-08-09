using Foundation.Extensions.Models;
using System;

namespace Ordering.Api.ServicesModels
{
    public class UpdateOrderItemStatusServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public Guid? OrderStatusId { get; set; }
        public string OrderStatusComment { get; set; }
    }
}
