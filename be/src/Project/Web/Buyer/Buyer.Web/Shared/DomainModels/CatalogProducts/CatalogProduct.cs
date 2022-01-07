using System;
using System.Collections.Generic;

namespace Buyer.Web.Shared.DomainModels.CatalogProducts
{
    public class CatalogProduct
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public bool IsNew { get; set; }
        public Guid SellerId { get; set; }
        public string BrandName { get; set; }
        public IEnumerable<Guid> Images { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
