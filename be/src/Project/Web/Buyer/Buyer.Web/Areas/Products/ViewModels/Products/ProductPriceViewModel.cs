using System.Collections.Generic;

namespace Buyer.Web.Areas.Products.ViewModels.Products
{
    public class ProductPriceViewModel
    {
        public decimal Current { get; set; }
        public string Currency { get; set; }
        public decimal? Old { get; set; }
        public decimal? LowestPrice { get; set; }
        public IEnumerable<ProductPriceInclusionViewModel> PriceInclusions { get; set; }
    }
}
