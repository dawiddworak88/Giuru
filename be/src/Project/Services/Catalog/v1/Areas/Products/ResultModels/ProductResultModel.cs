using Foundation.Extensions.Models;

namespace Catalog.Api.v1.Areas.Products.ResultModels
{
    public class ProductResultModel : BaseServiceResultModel
    {
        public Foundation.Database.Areas.Products.Entities.Product Product { get; set; }
    }
}
