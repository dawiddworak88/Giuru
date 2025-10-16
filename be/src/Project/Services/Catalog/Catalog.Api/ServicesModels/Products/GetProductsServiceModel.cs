using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.ServicesModels.Products
{
    public class GetProductsServiceModel : PagedBaseServiceModel
    {
        public Guid? CategoryId { get; set; }
        public bool? HasPrimaryProduct { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsSeller { get; set; }
    }
}
