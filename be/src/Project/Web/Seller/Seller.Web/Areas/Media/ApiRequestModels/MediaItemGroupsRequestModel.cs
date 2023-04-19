using Foundation.ApiExtensions.Models.Request;
using Seller.Web.Areas.Products.ApiRequestModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Media.ApiRequestModels
{
    public class MediaItemGroupsRequestModel : RequestModelBase
    {
        public IEnumerable<FileRequestModel> Files { get; set; }
        public IEnumerable<Guid> ClientGroupIds { get; set; }
    }
}
