using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class DiscountCodeEligibilityRequestModel : RequestModelBase
    {
        public Guid? ClientId { get; set; }

        public string Code { get; set; }
    }
}
