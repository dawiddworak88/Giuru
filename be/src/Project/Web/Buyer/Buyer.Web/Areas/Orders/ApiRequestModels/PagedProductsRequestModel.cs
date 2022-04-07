using Foundation.ApiExtensions.Models.Request;

namespace Buyer.Web.Areas.Orders.ApiRequestModels
{
    public class PagedProductsRequestModel : PagedRequestModelBase
    {
        public bool? HasPrimaryProduct { get; set; }
    }
}
