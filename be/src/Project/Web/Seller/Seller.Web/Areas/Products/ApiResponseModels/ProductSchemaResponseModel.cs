using Foundation.ApiExtensions.Models.Response;
using System;

namespace Seller.Web.Areas.Products.ApiResponseModels
{
    public class ProductSchemaResponseModel : BaseResponseModel
    {
        public string Name { get; set; }
        public string JsonSchema { get; set; }
        public string UiSchema { get; set; }
    }
}
