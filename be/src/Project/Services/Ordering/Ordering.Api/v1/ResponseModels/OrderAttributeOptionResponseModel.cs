using System;

namespace Ordering.Api.v1.ResponseModels
{
    public class OrderAttributeOptionResponseModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public Guid? Value { get; set; }
        public Guid? AttributeId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
