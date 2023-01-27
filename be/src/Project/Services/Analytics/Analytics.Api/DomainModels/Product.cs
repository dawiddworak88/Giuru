using System.Collections.Generic;
using System;

namespace Analytics.Api.DomainModels
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Ean { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
