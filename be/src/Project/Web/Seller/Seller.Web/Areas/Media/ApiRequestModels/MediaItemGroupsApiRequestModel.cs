using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Media.ApiRequestModels
{
    public class MediaItemGroupsApiRequestModel : RequestModelBase
    {
        public Guid? Id { get; set; }
        public IEnumerable<Guid> GroupIds { get; set; }
    }
}
