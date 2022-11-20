using System.Collections.Generic;

namespace Ordering.Api.v1.RequestModels
{
    public class SyncOrderLinesStatusesRequestModel
    {
        public IEnumerable<OrderItemStatusUpdatedRequestModel> OrderItems { get; set; }
    }
}
