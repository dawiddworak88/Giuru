using Foundation.ApiExtensions.Models.Response;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Products.ApiResponseModels
{
    public class BrandResponseModel : BaseResponseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
