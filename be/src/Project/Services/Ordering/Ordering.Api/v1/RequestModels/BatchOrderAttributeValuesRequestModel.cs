using System;
using System.Collections.Generic;

namespace Ordering.Api.v1.RequestModels
{
    public class BatchOrderAttributeValuesRequestModel
    {
        public Guid? OrderId { get; set; }
        public IEnumerable<BatchOrderAttributeValueRequestModel> Values { get; set; }
    }
}
