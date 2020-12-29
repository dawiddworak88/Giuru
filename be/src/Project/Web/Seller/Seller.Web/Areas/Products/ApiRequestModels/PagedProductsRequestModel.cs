using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class PagedProductsRequestModel : PagedRequestModelBase
    {        
        public bool IncludeProductVariants { get; set; }
    }
}
