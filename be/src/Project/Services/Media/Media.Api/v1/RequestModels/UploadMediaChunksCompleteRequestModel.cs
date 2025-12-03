using Foundation.ApiExtensions.Models.Request;
using System;

namespace Media.Api.v1.RequestModels
{
    public class UploadMediaChunksCompleteRequestModel : RequestModelBase
    {
        public Guid UploadId { get; set; }
        public string Filename { get; set; }
    }
}
