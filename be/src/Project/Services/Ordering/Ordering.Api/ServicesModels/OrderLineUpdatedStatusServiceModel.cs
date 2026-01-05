using System;

namespace Ordering.Api.ServicesModels
{
    public class OrderLineUpdatedStatusServiceModel
    {
        public Guid OrderId { get; set; }
        public int OrderLineIndex { get; set; }
        public Guid? PreviousStatusId { get; set; }
        public Guid? PreviousStateId { get; set; }
        public Guid? NewStatusId { get; set; }
        public Guid? NewStateId { get; set; }
    }
}
