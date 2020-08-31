using Foundation.Extensions.Models;

namespace Catalog.Api.v1.Areas.Products.Models
{
    public class GetProductsModel : BaseServiceModel
    {
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
        public string SearchTerm { get; set; }
    }
}
