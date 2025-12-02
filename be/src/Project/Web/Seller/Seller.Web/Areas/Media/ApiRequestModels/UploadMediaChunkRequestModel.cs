using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Media.ApiRequestModels
{
    public class UploadMediaChunkRequestModel : FileRequestModelBase
    {
        public Guid UploadId { get; set; }
    }
}
