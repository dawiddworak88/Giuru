using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.DownloadCenter.ApiRequestModels
{
    public class DownloadCenterItemRequestModel : RequestModelBase
    {
        public IEnumerable<Guid> CategoriesIds { get; set; }
        public IEnumerable<FileRequestModel> Files { get; set; }
    }
}
