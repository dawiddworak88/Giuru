using System.Collections.Generic;
using System;

namespace Ordering.Api.v1.RequestModels
{
    public class OrderItemStatusUpdatedRequestModel
    {
        public Guid OrderId { get; set; }
        public int OrderLineIndex { get; set; }
        public Guid StatusId { get; set; }
        public IEnumerable<OrderItemStatusUpdatedCommentTranslationRequestModel> CommentTranslations { get; set; }
    }
}
