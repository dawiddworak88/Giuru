using Foundation.ApiExtensions.Models.Response;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.ApiResponseModels
{
    public class ValidateOrderResponseModel : BaseResponseModel
    {
        public IEnumerable<string> ValidationMessages { get; set; }
    }
}
