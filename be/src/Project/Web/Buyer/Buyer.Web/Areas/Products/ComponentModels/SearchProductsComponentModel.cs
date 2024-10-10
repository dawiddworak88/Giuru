using Foundation.PageContent.ComponentModels;

namespace Buyer.Web.Areas.Products.ComponentModels
{
    public class SearchProductsComponentModel : ComponentModelBase
    {
        public string SearchTerm { get; set; }
        public string SearchArea { get; set; }
    }
}
