using Foundation.ApiExtensions.Models.Request;
using System;
using System.Text.Json;

namespace Tenant.Portal.Areas.Products.ApiRequestModels
{
    public class SaveProductRequestModel : BaseRequestModel
    {
        public Guid? Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public JsonElement FormData { get; set; }
    }
}
