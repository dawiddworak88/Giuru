using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Client.Api.v1.RequestModels
{
    public class DiscountCodeRequestModel : RequestModelBase
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }
        public IEnumerable<Guid> ClientIds { get; set; }
    }
}
