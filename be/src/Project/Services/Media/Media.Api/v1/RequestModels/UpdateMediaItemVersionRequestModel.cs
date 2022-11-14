using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Media.Api.v1.RequestModels
{
    public class UpdateMediaItemVersionRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaData { get; set; }
        public IEnumerable<Guid> ClientGroupIds { get; set; }
    }
}
