using Foundation.ApiExtensions.Models.Request;
using System;

namespace Api.v1.Areas.Products.RequestModels
{
    public class ProductRequestModel : BaseRequestModel
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public Guid? SchemaId { get; set; }
        public string FormData { get; set; }
    }
}
