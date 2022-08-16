using Foundation.Extensions.Models;
using System;

namespace Ordering.Api.ServicesModels
{
    public class GetOrderItemStatusChangesServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
