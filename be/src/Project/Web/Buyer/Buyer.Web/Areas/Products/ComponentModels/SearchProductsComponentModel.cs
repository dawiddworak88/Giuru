using Foundation.Search.Models;

namespace Buyer.Web.Areas.Products.ComponentModels
{
    public class SearchProductsComponentModel : PriceComponentModel
    {
        public string SearchTerm { get; set; }
        public QueryFilters Filters { get; set; }
    }
}
