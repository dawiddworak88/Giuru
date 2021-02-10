using System;

namespace Ordering.Api.v1.ResponseModels
{
    public class OrderStatusResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
