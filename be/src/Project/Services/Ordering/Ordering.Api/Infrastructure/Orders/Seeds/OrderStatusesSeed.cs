using Foundation.GenericRepository.Extensions;
using Microsoft.Extensions.Configuration;
using Ordering.Api.Infrastructure.Orders.Definitions;
using Ordering.Api.Infrastructure.Orders.Entities;
using System;
using System.Linq;
using System.Web;

namespace Ordering.Api.Infrastructure.Orders.Seeds
{
    public static class OrderStatusesSeed
    {
        public static void SeedOrderStatuses(OrderingContext context, IConfiguration configuration)
        {
            if (!string.IsNullOrWhiteSpace(configuration["OrderStatuses"]))
            {
                var orderStatuses = HttpUtility.UrlDecode(configuration["OrderStatuses"]).Split(";");

                foreach (var orderStatus in orderStatuses)
                {
                    var orderStatusProperties = orderStatus.Split("&");

                    SeedOrderStatus(
                        context,
                        Guid.Parse(orderStatusProperties[OrderStatusesConstants.OrderIdIndex]),
                        Guid.Parse(orderStatusProperties[OrderStatusesConstants.OrderStateIdIndex]),
                        int.Parse(orderStatusProperties[OrderStatusesConstants.OrderIndex]),
                        orderStatusProperties[OrderStatusesConstants.EnNameIndex],
                        orderStatusProperties[OrderStatusesConstants.DeNameIndex],
                        orderStatusProperties[OrderStatusesConstants.PlNameIndex]);
                }
            }
        }

        public static void SeedOrderStatus(
            OrderingContext context, 
            Guid id, 
            Guid orderStateId, 
            int order,
            string enName,
            string deName,
            string plName)
        {
            if (!context.OrderStatuses.Any(x => x.Id == id && x.IsActive))
            {
                var orderStatus = new OrderStatus
                {
                    Id = id,
                    Order = order,
                    OrderStateId = orderStateId
                };

                context.OrderStatuses.Add(orderStatus.FillCommonProperties());

                var enOrderStatusTranslation = new OrderStatusTranslation
                {
                    Name = enName,
                    OrderStatusId = orderStatus.Id,
                    Language = "en"
                };

                var deOrderStatusTranslation = new OrderStatusTranslation
                {
                    Name = deName,
                    OrderStatusId = orderStatus.Id,
                    Language = "de"
                };


                var plOrderStatusTranslation = new OrderStatusTranslation
                {
                    Name = plName,
                    OrderStatusId = orderStatus.Id,
                    Language = "pl"
                };

                context.OrderStatusTranslations.Add(enOrderStatusTranslation.FillCommonProperties());
                context.OrderStatusTranslations.Add(deOrderStatusTranslation.FillCommonProperties());
                context.OrderStatusTranslations.Add(plOrderStatusTranslation.FillCommonProperties());

                context.SaveChanges();

            }
        }
    }
}
