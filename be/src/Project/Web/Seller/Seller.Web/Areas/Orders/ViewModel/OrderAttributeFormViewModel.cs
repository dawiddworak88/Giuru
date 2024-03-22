using System.Collections.Generic;
using System;

namespace Seller.Web.Areas.Orders.ViewModel
{
    public class OrderAttributeFormViewModel
    {
        public Guid? Id { get; set; }
        public string IdLabel { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsOrderItemAttribute { get; set; }
        public string NameLabel { get; set; }
        public string TypeLabel { get; set; }
        public string OrderItemAttributeLabel { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string FieldRequiredErrorMessage { get; set; }
        public string SaveText { get; set; }
        public string SaveUrl { get; set; }
        public string EditUrl { get; set; }
        public string OrderAttributesUrl { get; set; }
        public string NavigateToAttributesText { get; set; }
        public IEnumerable<OrderAttributeTypeViewModel> Types { get; set; }
    }
}
