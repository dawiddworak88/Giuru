using System.Collections.Generic;

namespace Seller.Web.Areas.Products.ApiResponseModels
{
    public class ProductCardDefinitionResponseModel
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public IEnumerable<ProductCardDefinitionItemResponseModel> AnyOf { get; set; }
    }
}
