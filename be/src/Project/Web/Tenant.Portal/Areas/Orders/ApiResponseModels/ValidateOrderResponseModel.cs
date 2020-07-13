using Foundation.ApiExtensions.Models.Response;
using System.Collections.Generic;

namespace Seller.Portal.Areas.Orders.ApiResponseModels
{
    public class ValidateOrderResponseModel : BaseResponseModel
    {
        public IEnumerable<string> ValidationMessages { get; set; }
    }
}
