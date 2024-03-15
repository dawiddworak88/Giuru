using System;

namespace Seller.Web.Areas.Orders.DomainModels
{
    public class ApiOrderAttributeValue
    {
        public string Value { get; set; }
        public Guid? AttributeId { get; set; }
    }
}
