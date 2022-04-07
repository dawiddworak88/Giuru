using System;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ProductAttributeItemFormViewModel
    {
        public string Title { get; set; }
        public Guid? ProductAttributeId { get; set; }
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string NameRequiredErrorMessage { get; set; }
        public string NameLabel { get; set; }
        public string SaveText { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string ProductAttributeUrl { get; set; }
        public string NavigateToProductAttributesLabel { get; set; }
        public string SaveUrl { get; set; }
    }
}
