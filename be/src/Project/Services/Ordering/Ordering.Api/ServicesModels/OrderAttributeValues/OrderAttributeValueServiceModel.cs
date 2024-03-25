using System;

namespace Ordering.Api.ServicesModels.OrderAttributeValues
{
    public class OrderAttributeValueServiceModel
    {
        public Guid? Id { get; set; }
        public string Value { get; set; }
        public Guid? AttributeId { get; set; }
        public Guid? OrderItemId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}