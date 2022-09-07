using System;

namespace Catalog.Api.ServicesModels.Products
{
    public class ProductFileServiceModel
    {
        public Guid? Id { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
