using Foundation.Extensions.Models;

namespace Catalog.Api.v1.Areas.Products.ResultModels
{
    public class ProductResultModel : BaseServiceResultModel
    {
        public Catalog.Api.Infrastructure.Products.Entities.Product Product { get; set; }
    }
}
