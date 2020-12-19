using Foundation.Extensions.Models;

namespace Catalog.Api.v1.Areas.Categories.Models
{
    public class GetCategoriesModel : PagedBaseServiceModel
    {
        public int? Level { get; set; }
        public bool? LeafOnly { get; set; }
    }
}
