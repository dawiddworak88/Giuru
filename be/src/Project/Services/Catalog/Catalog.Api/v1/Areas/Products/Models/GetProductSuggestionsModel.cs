using Foundation.Extensions.Models;

namespace Catalog.Api.v1.Areas.Products.Models
{
    public class GetProductSuggestionsModel : BaseServiceModel
    {
        public string SearchTerm { get; set; }
        public int Size { get; set; }
    }
}
