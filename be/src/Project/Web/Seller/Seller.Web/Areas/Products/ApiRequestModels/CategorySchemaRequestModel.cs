using System;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class CategorySchemaRequestModel
    {
        public Guid? Id { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
        public string Language { get; set; }
    }
}
