using Foundation.ApiExtensions.Models.Request;
using System;

namespace Catalog.Api.v1.Products.RequestModels
{
    public class ProductAttributeItemRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public Guid? ProductAttributeId { get; set; }
    }
}
