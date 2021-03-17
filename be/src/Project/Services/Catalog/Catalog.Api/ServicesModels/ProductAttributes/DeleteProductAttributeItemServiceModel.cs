using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.ServicesModels.ProductAttributes
{
    public class DeleteProductAttributeItemServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
