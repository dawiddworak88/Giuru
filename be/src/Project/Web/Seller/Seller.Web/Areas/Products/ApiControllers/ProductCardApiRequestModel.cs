using System;

namespace Seller.Web.Areas.Products.ApiControllers
{
    public class ProductCardApiRequestModel
    {
        public Guid? CategoryId { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
    }
}
