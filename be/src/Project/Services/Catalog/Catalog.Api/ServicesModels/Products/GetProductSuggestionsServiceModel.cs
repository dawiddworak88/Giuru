using Foundation.Extensions.Models;

namespace Catalog.Api.ServicesModels.Products
{
    public class GetProductSuggestionsServiceModel : BaseServiceModel
    {
        public string SearchTerm { get; set; }
        public int Size { get; set; }
    }
}
