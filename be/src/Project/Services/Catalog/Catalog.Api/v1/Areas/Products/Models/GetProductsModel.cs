using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.v1.Areas.Products.Models
{
    public class GetProductsModel : PagedBaseServiceModel
    {
        public Guid? CategoryId { get; set; }
    }
}
