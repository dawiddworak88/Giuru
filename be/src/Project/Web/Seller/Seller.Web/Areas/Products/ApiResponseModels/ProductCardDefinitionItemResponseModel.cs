using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Products.ApiResponseModels
{
    public class ProductCardDefinitionItemResponseModel
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public IEnumerable<Guid> Enum { get; set; }
    }
}
