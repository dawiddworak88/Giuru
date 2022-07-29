using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.v1.RequestModel
{
    public class DownloadCenterItemRequestModel : RequestModelBase
    {
        public IEnumerable<Guid> CategoriesIds { get; set; }
        public IEnumerable<FileRequestModel> Files { get; set; }
    }
}
