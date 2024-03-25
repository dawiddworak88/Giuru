using Foundation.Extensions.Models;
using System;

namespace Ordering.Api.ServicesModels.OrderItems
{
    public class GetOrderItemServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
