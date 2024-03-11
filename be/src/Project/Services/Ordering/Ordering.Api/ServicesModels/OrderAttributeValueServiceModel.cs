using System;

namespace Ordering.Api.ServicesModels
{
    public class OrderAttributeValueServiceModel
    {
        public Guid? Id { get; set; }
        public string Value { get; set; }
        public Guid? AttributeId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
