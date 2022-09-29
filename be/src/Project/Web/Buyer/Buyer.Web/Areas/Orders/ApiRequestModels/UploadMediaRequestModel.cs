using Foundation.ApiExtensions.Models.Request;
using Microsoft.AspNetCore.Http;
using System;

namespace Buyer.Web.Areas.Orders.ApiRequestModels
{
    public class UploadMediaRequestModel
    {
        public Guid? BasketId { get; set; }
        public IFormFile File { get; set; }
    }
}
