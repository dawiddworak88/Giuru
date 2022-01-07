using Foundation.ApiExtensions.Models.Request;

namespace Buyer.Web.Shared.ApiRequestModels.Catalogs
{
    public class PagedCatalogProductsRequestModel : PagedRequestModelBase
    {
        public bool? HasPrimaryProduct { get; set; }
        public bool? IsNew { get; set; }
    }
}
