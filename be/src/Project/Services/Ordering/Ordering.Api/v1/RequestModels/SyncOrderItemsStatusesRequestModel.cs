using System.Collections.Generic;

namespace Ordering.Api.v1.RequestModels
{
    public class SyncOrderItemsStatusesRequestModel
    {
        public IEnumerable<SyncOrderItemStatusRequestModel> OrderItems { get; set; }
    }
}
