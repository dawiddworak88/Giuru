using Foundation.Extensions.Models;
using System;

namespace Feature.Product.Models
{
    public class CreateProductModel : BaseServiceModel
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public Guid? SchemaId { get; set; }
        public string FormData { get; set; }
    }
}
