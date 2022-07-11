using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.DownloadCenter.ApiRequestModels
{
    public class DownloadCenterItemRequestModel : RequestModelBase
    {
        public Guid? CategoryId { get; set; }
        public int? Order { get; set; }
    }
}
