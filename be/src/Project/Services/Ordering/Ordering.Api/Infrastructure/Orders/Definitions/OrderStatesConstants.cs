using System;

namespace Ordering.Api.Infrastructure.Orders.Definitions
{
    public static class OrderStatesConstants
    {
        public static readonly Guid NewId = Guid.Parse("a73d4e41-1ff0-4eb5-929d-ed0aa1f52c2a");
        public static readonly Guid PendingPaymentId = Guid.Parse("1056602b-acdb-4ed0-beac-224fdbe278c3");
        public static readonly Guid ProcessingId = Guid.Parse("77e3aee6-6053-4d95-bbb5-fc2ebfa4f0de");
        public static readonly Guid CompleteId = Guid.Parse("e071521b-682b-4a69-9070-be5782cbb223");
        public static readonly Guid ClosedId = Guid.Parse("877d5f96-871f-406e-8d8b-d32bea48bf40");
        public static readonly Guid CanceledId = Guid.Parse("5c192db0-af69-49b5-aebd-be1a02fc93ab");
        public static readonly Guid OnHoldId = Guid.Parse("b2dc9bd0-444f-4e96-8708-715f7e416e18");
        public static readonly Guid PaymentReviewId = Guid.Parse("cd4704c5-64c7-4933-b353-b6e366ccdc8f");
    }
}
