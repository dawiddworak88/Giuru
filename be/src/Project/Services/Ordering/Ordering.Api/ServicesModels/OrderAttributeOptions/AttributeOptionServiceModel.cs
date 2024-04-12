using System;

namespace Ordering.Api.ServicesModels.OrderAttributeOptions
{
    public class AttributeOptionServiceModel
    {
        public Guid? OrderAttributeOptionSetId { get; set; }
        public string Name { get; set; }
        public Guid? Value { get; set; }
    }
}
