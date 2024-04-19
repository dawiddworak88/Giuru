using Foundation.ApiExtensions.Models.Request;

namespace Buyer.Web.Shared.ApiRequestModels.Catalogs
{
    public class GetProductsBySkusRequestModel : PagedRequestModelBase
    {
        public string Skus { get; set; }
    }
}
