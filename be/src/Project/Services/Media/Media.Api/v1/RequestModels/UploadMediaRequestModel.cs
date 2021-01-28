using Foundation.ApiExtensions.Models.Request;
using Microsoft.AspNetCore.Http;

namespace Media.Api.v1.Areas.Media.RequestModels
{
    public class UploadMediaRequestModel : RequestModelBase
    {
        public IFormFile File { get; set; }
    }
}
