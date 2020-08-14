using Foundation.ApiExtensions.Models.Request;
using System;
using System.Text.Json;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class SaveProductRequestModel : RequestModelBase
    {
        public Guid? Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public Guid? SchemaId { get; set; }
        public JsonElement FormData { get; set; }
    }
}
