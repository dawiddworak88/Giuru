using Foundation.ApiExtensions.Models.Response;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.ApiResponseModels
{
    public class BasketApiResponseModel : BaseResponseModel
    {
        public string DiscountCode { get; set; }
        public IEnumerable<BasketItemApiResponseModel> Items { get; set; }
    }
}
