using Foundation.ApiExtensions.Models.Request;

namespace Api.v1.Areas.Products.RequestModels
{
    public class GetProductsRequestModel : BaseRequestModel
    {
        public string SearchTerm { get; set; }
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
