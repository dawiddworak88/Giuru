using System;

namespace Ordering.Api.v1.ResponseModels
{
    public class OrderResponseModel
    {
        public Guid? Id { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
