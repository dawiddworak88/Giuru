using System;

namespace Product.Api.v1.RequestModels
{
    public class ProductRequestModel
    {
        public Guid? Id { get; set; }
        public string Sku { get; set; }
        public string Language { get; set; }
    }
}
