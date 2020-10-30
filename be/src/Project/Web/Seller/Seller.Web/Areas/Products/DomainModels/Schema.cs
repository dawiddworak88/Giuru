using System;

namespace Seller.Web.Areas.Products.DomainModels
{
    public class Schema
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string JsonSchema { get; set; }
        public string UiSchema { get; set; }
    }
}
