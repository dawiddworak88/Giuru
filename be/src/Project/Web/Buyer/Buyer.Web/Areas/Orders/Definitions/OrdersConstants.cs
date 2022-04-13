using System;

namespace Buyer.Web.Areas.Orders.Definitions
{
    public static class OrdersConstants
    {
        public struct Basket
        {
            public static readonly int BasketProductImageMaxWidth = 250;
            public static readonly int BasketProductImageMaxHeight = 100;
        }

        public struct OrderStatuses
        {
            public static readonly Guid NewId = Guid.Parse("287ee71a-d87f-4563-833a-8e2771d1e5a5");
        }
    }
}
