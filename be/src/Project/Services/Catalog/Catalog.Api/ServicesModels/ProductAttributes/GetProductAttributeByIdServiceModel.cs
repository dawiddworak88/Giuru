using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.ServicesModels.ProductAttributes
{
    public class GetProductAttributeByIdServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
