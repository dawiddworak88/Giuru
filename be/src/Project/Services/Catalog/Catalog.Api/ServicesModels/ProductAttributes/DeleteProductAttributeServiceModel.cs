using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.ServicesModels.ProductAttributes
{
    public class DeleteProductAttributeServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
