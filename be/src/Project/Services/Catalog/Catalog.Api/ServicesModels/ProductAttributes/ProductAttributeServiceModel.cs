using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Catalog.Api.ServicesModels.ProductAttributes
{
    public class ProductAttributeServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public int? Order { get; set; }
        public IEnumerable<ProductAttributeItemServiceModel> ProductAttributeItems { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
