using Foundation.ApiExtensions.Models.Request;
using Seller.Web.Shared.ApiRequestModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class SaveProductRequestModel : RequestModelBase
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public Guid? CategoryId { get; set; }
        public string Description { get; set; }
        public Guid? PrimaryProductId { get; set; }
        public IEnumerable<SaveFileRequestModel> Images { get; set; }
        public IEnumerable<SaveFileRequestModel> Videos { get; set; }
        public IEnumerable<SaveFileRequestModel> Files { get; set; }
        public bool IsNew { get; set; }
        public string Ean { get; set; }
        public int DaysToFulfillment { get; set; }
        public bool IsPublished { get; set; }
        public string FormData { get; set; }
    }
}
