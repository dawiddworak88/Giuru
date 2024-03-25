using System;

namespace Ordering.Api.ServicesModels.OrderItems
{
    public class UpdateOrderItemsStatusServiceModel
    {
        public Guid Id { get; set; }
        public Guid StatusId { get; set; }
        public string StatusChangeComment { get; set; }
        public string Language { get; set; }
    }
}
