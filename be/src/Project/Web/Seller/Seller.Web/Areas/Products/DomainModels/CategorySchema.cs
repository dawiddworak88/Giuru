using System;

namespace Seller.Web.Areas.Products.DomainModels
{
    public class CategorySchema
    {
        public Guid Id { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
        public string Language { get; set; }
    }
}
