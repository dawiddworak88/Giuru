using Foundation.Extensions.Models;

namespace Catalog.Api.ServicesModels.Categories
{
    public class GetCategoriesServiceModel : PagedBaseServiceModel
    {
        public int? Level { get; set; }
        public bool? LeafOnly { get; set; }
    }
}
