using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.ServicesModels.ProductAttributes
{
    public class GetProductAttributeItemsServiceModel : PagedBaseServiceModel
    {
        public Guid? ProductAttributeId { get; set; }
    }
}
