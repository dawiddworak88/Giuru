using Foundation.ApiExtensions.Models.Response;
using System;

namespace Catalog.Api.v1.Products.ResponseModels
{
    public class ProductAttributeItemResponseModel : BaseResponseModel
    {
        public Guid? ProductAttributeId { get; set; }
        public string Name { get; set; }
        public int? Order { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
