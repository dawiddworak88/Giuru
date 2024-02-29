using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Catalog.Api.ServicesModels.Products
{
    public class CreateUpdateProductModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? PrimaryProductId { get; set; }
        public bool IsNew { get; set; }
        public bool IsPublished { get; set; }
        public bool IsProtected { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Sku { get; set; }
        public string FormData { get; set; }
        public string Ean { get; set; }
        public int FulfillmentTime { get; set; }
        public IEnumerable<Guid> Images { get; set; }
        public IEnumerable<Guid> Videos { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
