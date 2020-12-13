using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class PagedCategoriesRequestModel : PagedRequestModelBase
    {
        public bool? LeafOnly { get; set; }
    }
}
