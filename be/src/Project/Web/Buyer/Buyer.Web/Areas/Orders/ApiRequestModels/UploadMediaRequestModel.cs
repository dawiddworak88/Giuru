using Microsoft.AspNetCore.Http;

namespace Buyer.Web.Areas.Orders.ApiRequestModels
{
    public class UploadMediaRequestModel
    {
        public IFormFile File { get; set; }
        public string DiscountCode { get; set; }
    }
}
