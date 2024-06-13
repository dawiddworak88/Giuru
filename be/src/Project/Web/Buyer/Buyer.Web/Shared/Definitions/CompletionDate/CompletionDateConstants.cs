using System;

namespace Buyer.Web.Shared.Definitions.CompletionDate
{
    public static class CompletionDateConstants
    {
        public struct Zone
        {
            public static readonly Guid Id = Guid.Parse("5ec66556-b03e-4f63-80dd-08dc89162a08");
            public static readonly Guid FirstZoneId = Guid.Parse("97f99926-6e89-41b8-f7c3-08dc89162e18");
            public static readonly Guid SecondZoneId = Guid.Parse("ae520ca4-7594-4643-f7c4-08dc89162e18");
        }

        public struct Transport
        {
            public static readonly Guid Id = Guid.Parse("f14156a9-1910-4f7a-6c7d-08dc86be8445");
            public static readonly Guid OwnPickUpId = Guid.Parse("f8713556-8b00-4289-044a-08dc86be8bda");
            public static readonly Guid TrasnportEltapId = Guid.Parse("b8860fa5-e8a9-4fd4-044b-08dc86be8bda");
        }

        public struct Campaign
        {
            public static readonly Guid Id = Guid.Parse("0dd5730c-7ad8-4257-6c7e-08dc86be8445");
            public static readonly Guid TtewId = Guid.Parse("e3893a79-12a9-44b2-044c-08dc86be8bda");
            public static readonly Guid OtewId = Guid.Parse("069a1ca2-2352-4ab3-044d-08dc86be8bda");
        }

        public struct Condition
        {
            public static readonly Guid IsStockId = Guid.Parse("3516ba92-7a3a-4d58-ab94-6f42848864ae");
            public static readonly Guid FastDeliveryId = Guid.Parse("484da4d2-ac60-4a33-bf28-3f37949c4e0e");
            public static readonly Guid StandardId = Guid.Parse("d19787b5-cf60-4e42-bf11-2984923af246");
        }
    }
}
