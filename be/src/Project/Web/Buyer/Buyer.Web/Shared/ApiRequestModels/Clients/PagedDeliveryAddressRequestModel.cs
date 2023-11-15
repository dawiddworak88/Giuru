using Foundation.ApiExtensions.Models.Request;
using System;

namespace Buyer.Web.Shared.ApiRequestModels.Clients
{
    public class PagedDeliveryAddressesRequestModel : PagedRequestModelBase
    {
        public Guid? ClientId { get; set; }
    }
}
