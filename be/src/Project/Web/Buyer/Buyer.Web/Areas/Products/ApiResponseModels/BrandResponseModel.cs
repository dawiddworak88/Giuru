using Foundation.ApiExtensions.Models.Response;
using System;

namespace Buyer.Web.Areas.Products.ApiResponseModels
{
    public class BrandResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
