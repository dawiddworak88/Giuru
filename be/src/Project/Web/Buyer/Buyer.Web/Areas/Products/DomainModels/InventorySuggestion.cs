using System;

namespace Buyer.Web.Areas.Products.DomainModels
{
    public class InventorySuggestion
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
    }
}
