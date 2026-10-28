using Ordering.Api.Infrastructure.Orders.Entities;
using System;
using System.Linq;

namespace Ordering.Api.Infrastructure
{
    public static class OrdersQueryableExtensions
    {
        public static IQueryable<Order> ApplySearch(this IQueryable<Order> orders, OrderingContext context, string searchTerm, bool isSeller)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return orders;
            }

            var term = searchTerm.Trim();

            if (Guid.TryParse(term, out var orderId))
            {
                return orders.Where(x => x.Id == orderId);
            }

            var orderIdsByExternalReference = context.OrderItems
                .Where(x => x.IsActive && x.ExternalReferencePrefix.StartsWith(term))
                .Select(x => x.OrderId);

            // Non-sellers only see their own organisation's orders, so the client name narrows nothing for them
            if (isSeller is false)
            {
                return orders.Where(x => orderIdsByExternalReference.Contains(x.Id));
            }

            var matchingOrderIds = context.Orders
                .Where(x => x.IsActive && x.ClientName.StartsWith(term))
                .Select(x => x.Id)
                .Union(orderIdsByExternalReference);

            return orders.Where(x => matchingOrderIds.Contains(x.Id));
        }
    }
}
