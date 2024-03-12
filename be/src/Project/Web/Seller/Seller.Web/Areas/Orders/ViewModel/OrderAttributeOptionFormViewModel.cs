using System;

namespace Seller.Web.Areas.Orders.ViewModel
{
    public class OrderAttributeOptionFormViewModel
    {
        public string Title { get; set; }
        public Guid? AttributeId { get; set; }
        public Guid? Id { get; set; }
        public string IdLabel { get; set; }
        public string Name { get; set; }
        public string NameLabel { get; set; }
        public string Value { get; set; }
        public string ValueLabel { get; set; }
        public string FieldRequiredErrorMessage { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string SaveUrl { get; set; }
        public string SaveText { get; set; }
        public string OrderAttributeUrl { get; set; }
        public string NavigateToAttributeText { get; set; }
    }
}
