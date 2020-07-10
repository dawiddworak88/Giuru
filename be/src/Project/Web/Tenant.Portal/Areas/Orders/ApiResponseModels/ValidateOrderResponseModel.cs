using Foundation.ApiExtensions.Models.Response;
using System.Collections.Generic;

namespace Tenant.Portal.Areas.Orders.ApiResponseModels
{
    public class ValidateOrderResponseModel : BaseResponseModel
    {
        public IEnumerable<string> ValidationMessages { get; set; }
    }
}
