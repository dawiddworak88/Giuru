using System;

namespace Ordering.Api.v1.ResponseModels
{
    public class OrderAttributeOptionResponseModel
    {
        public string Name { get; set; }
        public Guid? Value { get; set; }
    }
}
