using System;

namespace Seller.Web.Areas.Products.ApiResponseModels
{
    public class CategoryResponseModel
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string FormData { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
