using Foundation.ApiExtensions.Models.Request;
using Microsoft.AspNetCore.Http;

namespace Buyer.Web.Areas.Orders.ApiRequestModels
{
    public class UploadMediaRequestModel : RequestModelBase
    {
        public IFormFile File { get; set; }
    }
}
