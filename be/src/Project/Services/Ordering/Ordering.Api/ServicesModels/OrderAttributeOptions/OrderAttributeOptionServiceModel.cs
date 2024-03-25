using System;

namespace Ordering.Api.ServicesModels.OrderAttributeOptions
{
    public class OrderAttributeOptionServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public Guid? Value { get; set; }
        public Guid? OrderAttributeId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
