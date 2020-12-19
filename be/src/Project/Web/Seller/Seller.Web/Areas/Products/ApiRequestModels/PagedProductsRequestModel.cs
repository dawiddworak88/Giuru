using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class PagedProductsRequestModel : PagedRequestModelBase
    {
        public Guid? SellerId { get; set; }
        public bool IncludeProductVariants { get; set; }
    }
}
