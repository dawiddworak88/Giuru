using System.Collections.Generic;

namespace Buyer.Web.Areas.Orders.ViewModel
{
    public class OrderItemAttributeTableViewModel
    {
        public IEnumerable<string> Labels { get; set; }
        public IEnumerable<OrderItemAttributeViewModel> OrderItemsAttributes { get; set; }
    }
}
