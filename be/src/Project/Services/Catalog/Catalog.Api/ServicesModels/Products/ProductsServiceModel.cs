using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;

namespace Catalog.Api.ServicesModels.Products
{
    public class ProductsServiceModel
    {
        public PagedResults<IEnumerable<ProductServiceModel>> PagedProducts { get; set; }
    }
}
