using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Media.ApiRequestModels
{
    public class UpdateMediaItemVersionRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaData { get; set; }
        public IEnumerable<Guid> ClientGroupIds { get; set; }
    }
}
