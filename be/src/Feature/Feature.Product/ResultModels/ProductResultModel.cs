using Foundation.Extensions.Models;

namespace Feature.Product.ResultModels
{
    public class ProductResultModel : BaseServiceResultModel
    {
        public Foundation.TenantDatabase.Areas.Products.Entities.Product Product { get; set; }
    }
}
