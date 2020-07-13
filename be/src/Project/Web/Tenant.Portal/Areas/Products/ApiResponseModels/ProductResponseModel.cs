using Foundation.ApiExtensions.Models.Response;
using System;

namespace Seller.Portal.Areas.Products.ApiResponseModels
{
    public class ProductResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string FormData { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
