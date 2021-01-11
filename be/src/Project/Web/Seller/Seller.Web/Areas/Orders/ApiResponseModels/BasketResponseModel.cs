using Foundation.ApiExtensions.Models.Response;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.ApiResponseModels
{
    public class BasketResponseModel : BaseResponseModel
    {
        public IEnumerable<BasketItemResponseModel> Items { get; set; }
    }
}
