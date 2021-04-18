using System.Collections.Generic;

namespace Buyer.Web.Areas.Products.ApiResponseModels
{
    public class ProductAttributeResponseModel
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Values { get; set; }
    }
}
