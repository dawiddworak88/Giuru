using Foundation.ApiExtensions.Models.Request;

namespace Buyer.Web.Areas.Products.ApiRequestModels
{
    public class AvailableProductsInventorySuggestionsRequestModel : RequestModelBase
    {
        public string SearchTerm { get; set; }
        public int SuggestionsCount { get; set; }
    }
}
