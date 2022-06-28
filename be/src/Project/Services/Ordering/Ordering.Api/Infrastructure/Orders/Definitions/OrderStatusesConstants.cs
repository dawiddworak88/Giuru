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

        public static readonly Guid NewId = Guid.Parse("287ee71a-d87f-4563-833a-8e2771d1e5a5");
        public static readonly Guid ProcessingId = Guid.Parse("578480b3-15ef-492d-9f86-9827789c6804");
    }
}
