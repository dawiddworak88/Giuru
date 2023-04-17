using System;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class ProductCardRequestModel
    {
        public Guid? Id { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
    }
}
