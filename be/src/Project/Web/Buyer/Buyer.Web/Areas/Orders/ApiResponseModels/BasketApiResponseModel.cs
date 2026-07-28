using Foundation.ApiExtensions.Models.Response;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Orders.ApiResponseModels
{
    public class BasketApiResponseModel : BaseResponseModel
    {
        public string MoreInfo { get; set; }
        public string DiscountCode { get; set; }
        public IEnumerable<BasketItemApiResponseModel> Items { get; set; }
    }
}
