using Foundation.Extensions.Models;
using System;

namespace Ordering.Api.ServicesModels
{
    public class UpdateOrderStatusServiceModel : BaseServiceModel
    {
        public Guid? OrderId { get; set; }
        public Guid? OrderStatusId { get; set; }
        public bool IsSeller { get; set; }
    }
}
