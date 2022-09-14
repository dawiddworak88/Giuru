using System;

namespace Seller.Web.Areas.Orders.Definitions
{
    public static class OrdersConstants
    {
        public struct OrderStatuses
        {
            public static readonly Guid ProcessingId = Guid.Parse("578480b3-15ef-492d-9f86-9827789c6804");
        }

        public struct Basket
        {
            public static readonly int BasketProductImageMaxWidth = 250;
            public static readonly int BasketProductImageMaxHeight = 100;
        }
    }
}
