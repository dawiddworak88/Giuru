using Foundation.ApiExtensions.Models.Request;

namespace Tenant.Portal.Areas.Products.ApiRequestModels
{
    public class ProductsRequestModel : BaseRequestModel
    {
        public string SearchTerm { get; set; }
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
