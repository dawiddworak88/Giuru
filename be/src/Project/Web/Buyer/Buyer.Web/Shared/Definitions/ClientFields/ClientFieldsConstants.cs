using System;

namespace Buyer.Web.Shared.Definitions.CompletionDate
{
    public static class ClientFieldsConstants
    {
        public struct Zone
        {
            public static readonly Guid Id = Guid.Parse("f9f50ca7-c54f-4f11-4a20-08dc74ab4b1a");
        }

        public struct Transport
        {
            public static readonly Guid Id = Guid.Parse("35968b7b-0329-4def-b450-08dc65e9a01d");
            public static readonly Guid EltapTransportId = Guid.Parse("0caf6403-7e80-4b66-9d32-08dc65e9a5f8");
        }

        public struct Campaign
        {
            public static readonly Guid Id = Guid.Parse("89f22cda-ccc1-4102-4a1f-08dc74ab4b1a");
        }

        public struct Condition
        {
            public static readonly Guid IsStockId = Guid.Parse("3516ba92-7a3a-4d58-ab94-6f42848864ae");
            public static readonly Guid FastDeliveryId = Guid.Parse("484da4d2-ac60-4a33-bf28-3f37949c4e0e");
            public static readonly Guid StandardId = Guid.Parse("d19787b5-cf60-4e42-bf11-2984923af246");
        }
    }
}
