using Foundation.ApiExtensions.Models.Request;

namespace Buyer.Web.Areas.Products.ApiRequestModels
{
    public class ProductSuggestionsRequestModel : RequestModelBase
    {
        public string SearchTerm { get; set; }
        public int Size { get; set; }
    }
}
