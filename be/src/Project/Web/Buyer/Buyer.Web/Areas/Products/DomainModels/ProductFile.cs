using System;

namespace Buyer.Web.Areas.Products.DomainModels
{
    public class ProductFile
    {
        public Guid Id { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
