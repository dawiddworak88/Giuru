using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.DownloadCenter.ApiRequestModels
{
    public class DownloadCenterItemApiRequestModel : RequestModelBase
    {
        public IEnumerable<Guid> CategoriesIds { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
