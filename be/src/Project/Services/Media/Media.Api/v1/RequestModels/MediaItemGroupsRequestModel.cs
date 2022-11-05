using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Media.Api.v1.RequestModels
{
    public class MediaItemGroupsRequestModel : RequestModelBase
    {
        public IEnumerable<Guid> GroupIds { get; set; }
    }
}
