using System;

namespace Ordering.Api.v1.ResponseModels
{
    public class OrderFileResponseModel
    {
        public Guid? Id { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
