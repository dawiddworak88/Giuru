using System;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class ProductCardApiRequestModel
    {
        public Guid? CategoryId { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
    }
}
