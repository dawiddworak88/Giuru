using Foundation.ApiExtensions.Models.Response;
using Foundation.TenantDatabase.Areas.Products.Entities;
using System;

namespace Api.v1.Areas.Products.ResponseModels
{
    public class ProductResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Sku { get; set; }

        public ProductResponseModel()
        { 
        }

        public ProductResponseModel(Product product)
        {
            this.Id = product.Id;
            this.Sku = product.Sku;
            this.Name = product.Name;
        }
    }
}
