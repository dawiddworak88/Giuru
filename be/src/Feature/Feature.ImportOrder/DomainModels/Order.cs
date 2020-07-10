using System.Collections.Generic;

namespace Feature.ImportOrder.DomainModels
{
    public class Order
    {
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
