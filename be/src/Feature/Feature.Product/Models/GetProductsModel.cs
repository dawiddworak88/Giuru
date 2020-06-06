using Foundation.Extensions.Models;

namespace Feature.Product.Models
{
    public class GetProductsModel : BaseServiceModel
    {
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
        public string SearchTerm { get; set; }
    }
}
