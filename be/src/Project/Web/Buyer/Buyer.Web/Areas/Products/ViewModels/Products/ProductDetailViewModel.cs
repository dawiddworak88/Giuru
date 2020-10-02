using Buyer.Web.Shared.Files.ViewModels;
using Buyer.Web.Shared.Images.ViewModels;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Products.ViewModels.Products
{
    public class ProductDetailViewModel
    {
        public string Title { get; set; }
        public bool IsAuthenticated { get; set; }
        public string SignInUrl { get; set; }
        public string SkuLabel { get; set; }
        public string Sku { get; set; }
        public string ByLabel { get; set; }
        public string BrandName { get; set; }
        public string BrandUrl { get; set; }
        public string PricesLabel { get; set; }
        public string ProductInformationLabel { get; set; }
        public string SignInToSeePricesLabel { get; set; }
        public bool InStock { get; set; }
        public string DescriptionLabel { get; set; }
        public string Description { get; set; }
        public string InStockLabel { get; set; }
        public FilesViewModel Files { get; set; }
        public IEnumerable<ImageViewModel> Images { get; set; }
        public IEnumerable<ProductFeatureViewModel> Features { get; set; }
    }
}
