using Foundation.ApiExtensions.Models.Request;
using Microsoft.AspNetCore.Http;

namespace Media.Api.v1.Area.Media.ApiRequestModels
{
    public class UploadMediaRequestModel : RequestModelBase
    {
        public IFormFile File { get; set; }
    }
}
