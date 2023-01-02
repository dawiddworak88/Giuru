using System;

namespace Seller.Web.Areas.Orders.Definitions
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
            public static readonly Guid CancelId = Guid.Parse("4fcdefc0-8181-490c-9378-fe96d234ccc9");
        }
    }
}
