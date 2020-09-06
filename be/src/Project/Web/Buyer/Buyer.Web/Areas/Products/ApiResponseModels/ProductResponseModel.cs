using Foundation.ApiExtensions.Models.Response;
using System;

namespace Buyer.Web.Areas.Products.ApiResponseModels
{
    public class ProductResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
    }
}
