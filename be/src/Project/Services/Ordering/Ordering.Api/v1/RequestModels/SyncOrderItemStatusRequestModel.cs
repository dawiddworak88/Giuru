using System;

namespace Ordering.Api.v1.RequestModels
{
    public class SyncOrderItemStatusRequestModel
    {
        public Guid Id { get; set; }
        public Guid StatusId { get; set; }
    }
}
