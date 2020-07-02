using Foundation.ApiExtensions.Models.Request;
using System;

namespace Api.v1.Areas.Products.RequestModels
{
    public class ProductRequestModel : BaseRequestModel
    {
        public Guid? Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public Guid? SchemaId { get; set; }
        public string FormData { get; set; }
    }
}
