using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class ProductsRequestModel : RequestModelBase
    {
        public string SearchTerm { get; set; }
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
