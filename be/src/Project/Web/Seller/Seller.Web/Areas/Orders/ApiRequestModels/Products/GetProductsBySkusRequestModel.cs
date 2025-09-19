using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Orders.ApiRequestModels.Products
{
    public class GetProductsBySkusRequestModel : PagedRequestModelBase
    {
        public string Skus { get; set; }
    }
}
