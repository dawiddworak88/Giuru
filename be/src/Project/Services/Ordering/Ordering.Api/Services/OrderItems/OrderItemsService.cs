using Foundation.Localization;
using Microsoft.Extensions.Localization;
using Ordering.Api.Infrastructure;

namespace Ordering.Api.Services.OrderItems
{
    public class OrderItemsService : IOrderItemsService
    {
        private readonly OrderingContext _context;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;

        public OrderItemsService(
            OrderingContext context, 
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            _context = context;
            _orderLocalizer = orderLocalizer;
        }
    }
}
