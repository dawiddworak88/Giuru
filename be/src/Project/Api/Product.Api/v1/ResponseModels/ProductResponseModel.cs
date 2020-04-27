using System;

namespace Product.Api.v1.ResponseModels
{
    public class ProductResponseModel
    {
        public Guid Id { get; set; }

        public string Sku { get; set; }

        public ProductResponseModel(Foundation.TenantDatabase.Areas.Products.Entities.Product product)
        {
            this.Id = product.Id;
            this.Sku = product.Sku;
        }
    }
}
