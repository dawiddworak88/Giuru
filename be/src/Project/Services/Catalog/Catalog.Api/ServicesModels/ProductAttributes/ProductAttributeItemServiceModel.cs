using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.ServicesModels.ProductAttributes
{
    public class ProductAttributeItemServiceModel : BaseServiceModel
    {
        public Guid Id { get; set; }
        public Guid ProductAttributeId { get; set; }
        public string Name { get; set; }
        public int? Order { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
