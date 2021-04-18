using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;

namespace Catalog.Api.v1.Products.ResultModels
{
    public class ProductsResponseModel
    {
        public PagedResults<IEnumerable<ProductResponseModel>> PagedProducts { get; set; }
    }
}
