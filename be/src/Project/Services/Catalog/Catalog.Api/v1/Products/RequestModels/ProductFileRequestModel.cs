using System;

namespace Catalog.Api.v1.Products.RequestModels
{
    public class ProductFileRequestModel
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
    }
}
