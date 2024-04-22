using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class PagedAddressesRequestModel : PagedRequestModelBase
    {
        public Guid? ClientId { get; set; }
    }
}
