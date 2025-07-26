using System;
using System.Collections.Generic;

namespace Catalog.Api.ServicesModels.Products
{
    public class ProductServiceModel
    {
        public Guid? Id { get; set; }
        public Guid? PrimaryProductId { get; set; }
        public string PrimaryProductSku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsNew { get; set; }
        public bool IsPublished { get; set; }
        public bool IsProtected { get; set; }
        public string FormData { get; set; }
        public Guid SellerId { get; set; }
        public string BrandName { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Ean { get; set; }
        public IEnumerable<Guid> ProductVariants { get; set; }
        public IEnumerable<Guid> Images { get; set; }
        public IEnumerable<Guid> Videos { get; set; }
        public IEnumerable<Guid> Files { get; set; }
        public string Sku { get; set; }
        public int? FulfillmentTime { get; set; }
        public double? StockAvailableQuantity { get; set; }
        public double? OutletAvailableQuantity { get; set; }
        public IEnumerable<ProductAttributeServiceModel> ProductAttributes { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
