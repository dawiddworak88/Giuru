using System;

namespace Seller.Web.Areas.Orders.DomainModels
{
    public class ApiOrderAttributeValue
    {
        public Guid? OrderId { get; set; }
        public Guid? OrderItemId { get; set; }
        public Guid? AttributeId { get; set; }
        public string Value { get; set; }
    }
}
