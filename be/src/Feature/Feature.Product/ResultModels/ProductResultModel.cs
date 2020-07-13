using Foundation.Extensions.Models;

namespace Feature.Product.ResultModels
{
    public class ProductResultModel : BaseServiceResultModel
    {
        public Foundation.Database.Areas.Products.Entities.Product Product { get; set; }
    }
}
