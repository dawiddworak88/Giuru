using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class SaveProductApiRequestModel : RequestModelBase
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public Guid? CategoryId { get; set; }
        public string Description { get; set; }
        public Guid? PrimaryProductId { get; set; }
        public IEnumerable<Guid> Images { get; set; }
        public IEnumerable<Guid> Videos { get; set; }
        public IEnumerable<Guid> Files { get; set; }
        public bool IsNew { get; set; }
        public bool IsPublished { get; set; }
        public string Ean { get; set; }
        public int FulfillmentTime { get; set; }
        public string FormData { get; set; }
    }
}
