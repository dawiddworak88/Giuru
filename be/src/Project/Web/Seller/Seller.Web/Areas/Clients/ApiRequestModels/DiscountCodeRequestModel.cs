using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class DiscountCodeRequestModel : RequestModelBase
    {
        public string Version { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }
        public IEnumerable<Guid> ClientIds { get; set; }
    }
}
