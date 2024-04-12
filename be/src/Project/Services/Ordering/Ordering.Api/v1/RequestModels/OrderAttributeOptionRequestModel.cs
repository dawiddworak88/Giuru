using System;

namespace Ordering.Api.v1.RequestModels
{
    public class OrderAttributeOptionRequestModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public Guid? AttributeId { get; set; }
    }
}
