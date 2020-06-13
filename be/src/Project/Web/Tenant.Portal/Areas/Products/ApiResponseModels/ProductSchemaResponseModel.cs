using Foundation.ApiExtensions.Models.Response;
using System;

namespace Tenant.Portal.Areas.Products.ApiResponseModels
{
    public class ProductSchemaResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string JsonSchema { get; set; }
        public string UiSchema { get; set; }
    }
}
