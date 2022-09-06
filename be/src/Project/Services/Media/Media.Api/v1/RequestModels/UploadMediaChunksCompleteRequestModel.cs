using Foundation.ApiExtensions.Models.Request;

namespace Media.Api.v1.RequestModels
{
    public class UploadMediaChunksCompleteRequestModel : RequestModelBase
    {
        public string Filename { get; set; }
    }
}
