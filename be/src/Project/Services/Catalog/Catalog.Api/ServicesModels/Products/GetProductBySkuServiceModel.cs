using Foundation.Extensions.Models;

namespace Catalog.Api.ServicesModels.Products
{
    public class GetProductBySkuServiceModel : BaseServiceModel
    {
        public string Sku { get; set; }
        public bool? IsSeller { get; set; }
    }
}
