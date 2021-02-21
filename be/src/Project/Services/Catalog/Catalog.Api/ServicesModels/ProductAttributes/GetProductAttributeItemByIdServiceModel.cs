using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.ServicesModels.ProductAttributes
{
    public class GetProductAttributeItemByIdServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
