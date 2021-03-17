using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.ServicesModels.ProductAttributes
{
    public class CreateUpdateProductAttributeItemServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public Guid? ProductAttributeId { get; set; }
        public string Name { get; set; }
        public int? Order { get; set; }
    }
}
