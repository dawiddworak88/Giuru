using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class ProductRequestModel : RequestModelBase
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public Guid? SchemaId { get; set; }
        public string FormData { get; set; }
    }
}
