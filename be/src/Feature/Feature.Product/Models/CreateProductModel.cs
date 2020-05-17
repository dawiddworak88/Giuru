using Foundation.Extensions.Models;

namespace Feature.Product.Models
{
    public class CreateProductModel : BaseServiceModel
    {
        public string Name { get; set; }
        public string Sku { get; set; }
    }
}
