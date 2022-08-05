using System;

namespace Ordering.Api.v1.RequestModels
{
    public class OrderItemStatusCommentRequestModel
    {
        public Guid? OrderItemId { get; set; }
        public string OrderItemStatusComment { get; set; }
    }
}
