using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class PagedClientFieldValuesRequestModel : PagedRequestModelBase
    {
        public Guid? ClientId { get; set; }
    }
}
