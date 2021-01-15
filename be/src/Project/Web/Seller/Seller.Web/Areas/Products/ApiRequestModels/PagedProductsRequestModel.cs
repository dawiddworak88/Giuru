using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class PagedProductsRequestModel : PagedRequestModelBase
    {        
        public string Ids { get; set; }
        public bool? HasPrimaryProduct { get; set; }
    }
}
