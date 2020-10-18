using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;

namespace Catalog.Api.v1.Areas.Products.ResultModels
{
    public class ProductsResultModel
    {
        public PagedResults<IEnumerable<ProductResultModel>> PagedProducts { get; set; }
    }
}
