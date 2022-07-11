using Foundation.ApiExtensions.Models.Request;
using System;

namespace DownloadCenter.Api.v1.RequestModel
{
    public class DownloadCenterItemRequestModel : RequestModelBase
    {
        public Guid? CategoryId { get; set; }
        public int? Order { get; set; }
    }
}
