using Foundation.ApiExtensions.Models.Request;
using Microsoft.AspNetCore.Http;
using System;

namespace Seller.Web.Areas.Media.ApiRequestModels
{
    public class UploadMediaRequestModel : RequestModelBase
    {
        public IFormFile File { get; set; }
        public Guid ClientId { get; set; }
    }
}
 