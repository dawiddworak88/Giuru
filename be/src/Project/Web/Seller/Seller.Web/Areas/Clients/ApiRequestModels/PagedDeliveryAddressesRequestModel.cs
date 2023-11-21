using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class PagedDeliveryAddressesRequestModel : PagedRequestModelBase
    {
        public Guid? ClientId { get; set; }
    }
}
