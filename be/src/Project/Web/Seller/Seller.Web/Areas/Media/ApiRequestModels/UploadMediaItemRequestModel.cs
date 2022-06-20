using Foundation.ApiExtensions.Models.Request;
using Microsoft.AspNetCore.Http;

namespace Seller.Web.Areas.Media.ApiRequestModels
{
    public class UploadMediaItemRequestModel : RequestModelBase
    {
        public IFormFile File { get; set; }
    }
}
