using Seller.Web.Areas.Products.DomainModels;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ProductDetailFormViewModel
    {
        public Product Product { get; set; }
        public Schema Schema { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string NameLabel { get; set; }
        public string NameRequiredErrorMessage { get; set; }
        public string EnterNameText { get; set; }
        public string SelectSchemaLabel { get; set; }
        public string SkuLabel { get; set; }
        public string SkuRequiredErrorMessage { get; set; }
        public string EnterSkuText { get; set; }
        public string ProductDetailText { get; set; }
        public string SaveUrl { get; set; }
        public string SaveText { get; set; }
    }
}