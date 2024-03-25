using System;

namespace Ordering.Api.v1.RequestModels
{
    public class BatchOrderAttributeValueRequestModel
    {
        public Guid? AttributeId { get; set; }
        public string Value { get; set; }
    }
}
