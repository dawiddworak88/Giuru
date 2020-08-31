using Foundation.Extensions.Models;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;

namespace Catalog.Api.v1.Areas.Products.ResultModels
{
    public class ProductsResultModel : BaseServiceResultModel
    {
        public PagedResults<IEnumerable<Catalog.Api.Infrastructure.Products.Entities.Product>> Products { get; set; }
    }
}
