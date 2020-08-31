using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.v1.Areas.Products.Models
{
    public class CreateUpdateProductModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public Guid? SchemaId { get; set; }
        public string FormData { get; set; }
    }
}
