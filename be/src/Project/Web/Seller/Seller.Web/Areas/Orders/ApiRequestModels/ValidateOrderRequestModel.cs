using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Orders.ApiRequestModels
{
    public class ValidateOrderRequestModel : RequestModelBase
    {
        public Guid? ClientId { get; set; }
    }
}
