using Foundation.Extensions.Models;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;

namespace Feature.Product.ResultModels
{
    public class ProductsResultModel : BaseServiceResultModel
    {
        public PagedResults<IEnumerable<Foundation.Database.Areas.Products.Entities.Product>> Products { get; set; }
    }
}
