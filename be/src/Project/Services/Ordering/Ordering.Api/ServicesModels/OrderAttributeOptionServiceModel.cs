using System;

namespace Ordering.Api.ServicesModels
{
    public class OrderAttributeOptionServiceModel
    {
        public Guid? OrderAttributeOptionSetId { get; set; }
        public string Name { get; set; }
        public Guid? Value { get; set; }
    }
}
