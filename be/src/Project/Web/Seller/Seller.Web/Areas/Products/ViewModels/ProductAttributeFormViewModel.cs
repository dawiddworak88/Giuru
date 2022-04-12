using System;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ProductAttributeFormViewModel
    {
        public string Title { get; set; }
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string NameRequiredErrorMessage { get; set; }
        public string NameLabel { get; set; }
        public string SaveText { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string EditUrl { get; set; }
        public string SaveUrl { get; set; }
        public string AttributesUrl { get; set; }
        public string NavigateToAttributesLabel { get; set; }
        public string IdLabel { get; set; }
    }
}
