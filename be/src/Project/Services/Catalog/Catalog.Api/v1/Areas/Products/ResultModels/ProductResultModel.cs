using Foundation.ApiExtensions.Models.Response;
using System;

namespace Catalog.Api.v1.Areas.Products.ResultModels
{
    public class ProductResultModel : BaseResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAlt { get; set; }
        public string Sku { get; set; }
        public string FormData { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
