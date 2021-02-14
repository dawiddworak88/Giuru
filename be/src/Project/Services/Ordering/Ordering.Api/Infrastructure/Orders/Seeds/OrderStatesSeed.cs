using Foundation.GenericRepository.Extensions;
using Microsoft.Extensions.Configuration;
using Ordering.Api.Infrastructure.Orders.Definitions;
using Ordering.Api.Infrastructure.Orders.Entities;
using System;
using System.Linq;

namespace Ordering.Api.Infrastructure.Orders.Seeds
{
    public static class OrderStatesSeed
    {
        public static void SeedOrderStates(OrderingContext context)
        {
            SeedOrderState(context, OrderStatesConstants.NewId, "New");
            SeedOrderState(context, OrderStatesConstants.PendingPaymentId, "Pending Payment");
            SeedOrderState(context, OrderStatesConstants.ProcessingId, "Processing");
            SeedOrderState(context, OrderStatesConstants.CompleteId, "Complete");
            SeedOrderState(context, OrderStatesConstants.ClosedId, "Closed");
            SeedOrderState(context, OrderStatesConstants.CanceledId, "Canceled");
            SeedOrderState(context, OrderStatesConstants.OnHoldId, "On Hold");
            SeedOrderState(context, OrderStatesConstants.PaymentReviewId, "Payment Review");
        }

        private static void SeedOrderState(OrderingContext context, Guid id, string name)
        { 
            if (!context.OrderStates.Any(x => x.Id == id && x.IsActive))
            {
                var orderState = new OrderState
                { 
                    Id = id,
                    Name = name
                };

                context.OrderStates.Add(orderState.FillCommonProperties());

                context.SaveChanges();
            }
        }
    }
}
