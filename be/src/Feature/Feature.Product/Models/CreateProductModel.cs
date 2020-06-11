using Foundation.Extensions.Models;
using System;

namespace Feature.Product.Models
{
    public class CreateUpdateProductModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string FormData { get; set; }
    }
}
