using Feature.ImportOrder.DomainModels;
using Foundation.ApiExtensions.Models.Request;
using System;

namespace Tenant.Portal.Areas.Orders.ApiRequestModels
{
    public class ValidateOrderRequestModel : BaseRequestModel
    {
        public Guid? ClientId { get; set; }
        public Order Order { get; set; }
    }
}
