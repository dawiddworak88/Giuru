using System;

namespace Buyer.Web.Areas.Products.DomainModels
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
    }
}
