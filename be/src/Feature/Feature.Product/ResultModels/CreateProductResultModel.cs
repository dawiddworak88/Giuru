using Foundation.Extensions.Models;

namespace Feature.Product.ResultModels
{
    public class CreateProductResultModel : BaseServiceResultModel
    {
        public Foundation.TenantDatabase.Areas.Products.Entities.Product Product { get; set; }
    }
}
