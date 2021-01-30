using System;

namespace Ordering.Api.Infrastructure.Orders.Definitions
{
    public static class OrderStatusesConstants
    {
        public static readonly int OrderIdIndex = 0;
        public static readonly int OrderStateIdIndex = 1;
        public static readonly int OrderIndex = 2;
        public static readonly int EnNameIndex = 3;
        public static readonly int DeNameIndex = 4;
        public static readonly int PlNameIndex = 5;

        public static readonly Guid NewId = Guid.Parse("56BF671B-306A-41A0-8222-08D8C4A071F2");
    }
}
