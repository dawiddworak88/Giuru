using Foundation.ApiExtensions.Models.Request;
using System;

namespace Buyer.Web.Shared.ApiRequestModels.Clients
{
    public class DiscountCodeEligibilityRequestModel : RequestModelBase
    {
        public Guid? ClientId { get; set; }

        public string Code { get; set; }
    }
}
