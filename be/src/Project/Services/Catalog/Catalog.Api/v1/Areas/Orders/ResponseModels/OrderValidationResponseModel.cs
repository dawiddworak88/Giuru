using Foundation.ApiExtensions.Models.Response;
using System.Collections.Generic;

namespace Catalog.Api.v1.Areas.Orders.ResponseModels
{
    public class OrderValidationResponseModel : BaseResponseModel
    {
        public List<string> ValidationMessages { get; set; }

        public OrderValidationResponseModel()
        {
        }

        public OrderValidationResponseModel(List<string> errors)
        {
            this.ValidationMessages = new List<string>();
            this.ValidationMessages.AddRange(errors);
        }
    }
}
