using Foundation.ApiExtensions.Models.Request;
using Seller.Web.Areas.Products.DomainModels;
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
        public IEnumerable<ProductMediaFile> Images { get; set; }
        public IEnumerable<ProductMediaFile> Videos { get; set; }
        public IEnumerable<Guid> Files { get; set; }
        public bool IsNew { get; set; }
        public bool IsPublished { get; set; }
        public string Ean { get; set; }
        public string FormData { get; set; }
    }
}
