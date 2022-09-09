using System;

namespace Catalog.Api.v1.Products.ResponseModels
{
    public class ProductFileResponseModel
    {
        public Guid? Id { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
