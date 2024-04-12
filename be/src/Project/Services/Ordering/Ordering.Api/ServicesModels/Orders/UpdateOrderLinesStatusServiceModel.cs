using System;
using System.Collections.Generic;

namespace Ordering.Api.ServicesModels.Orders
{
    public class UpdateOrderLinesStatusServiceModel
    {
        public Guid Id { get; set; }
        public int OrderLineIndex { get; set; }
        public Guid StatusId { get; set; }
        public IEnumerable<UpdateOrderLineCommentServiceModel> CommentTranslations { get; set; }
    }
}
