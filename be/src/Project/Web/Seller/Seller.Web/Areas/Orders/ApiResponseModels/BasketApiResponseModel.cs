using Foundation.ApiExtensions.Models.Response;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.ApiResponseModels
{
    public class BasketApiResponseModel : BaseResponseModel
    {
        public IEnumerable<BasketItemApiResponseModel> Items { get; set; }
    }
}
