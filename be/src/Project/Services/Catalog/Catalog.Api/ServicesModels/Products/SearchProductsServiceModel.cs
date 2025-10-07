using Foundation.Extensions.Models;

namespace Catalog.Api.ServicesModels.Products
{
    public class SearchProductsServiceModel : PagedBaseServiceModel
    {
        public SearchProductsFiltersServiceModel Filters { get; set; }
        public bool? IsSeller { get; set; }
    }
}
