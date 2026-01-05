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
            public static readonly Guid CancelId = Guid.Parse("4fcdefc0-8181-490c-9378-fe96d234ccc9");
            public static readonly Guid HoldId = Guid.Parse("e9fa4fc3-36c7-48ec-bf90-df9901867a38");
            public static readonly Guid ClosedId = Guid.Parse("fc24e7e2-020b-4110-9415-7184ec837e71");
        }
    }
}
