using Foundation.ApiExtensions.Models.Request;
using System;

namespace Ordering.Api.v1.RequestModels
{
    public class UpdateOrderStatusRequestModel : RequestModelBase
    {
        public Guid? OrderId { get; set; }
        public Guid? OrderStatusId { get; set; }
    }
}
