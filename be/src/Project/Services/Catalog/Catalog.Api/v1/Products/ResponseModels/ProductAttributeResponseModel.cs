using Foundation.ApiExtensions.Models.Response;
using System;
using System.Collections.Generic;

namespace Catalog.Api.v1.Products.ResponseModels
{
    public class ProductAttributeResponseModel : BaseResponseModel
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public int? Order { get; set; }
        public IEnumerable<ProductAttributeItemResponseModel> Items { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
