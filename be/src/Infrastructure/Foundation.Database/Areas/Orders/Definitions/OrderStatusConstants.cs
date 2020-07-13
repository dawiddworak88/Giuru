using System;

namespace Foundation.Database.Areas.Orders.Definitions
{
    public static class OrderStatusConstants
    {
        public static Guid OrderStatusesTaxonomyId { get; } = Guid.Parse("e1b04379-1181-4b0d-9008-41b7ade28f7d");
        public const string OrderStatusTaxonomyName = "Order status";

        public static Guid NewOrderStatusTaxonomyId { get; } = Guid.Parse("42df698f-60b7-476e-b273-814cd98683a2");
        public const string NewOrderStatusName = "New";

        public static Guid ProcessingOrderStatusTaxonomyId { get; } = Guid.Parse("6be46874-9831-448f-ac52-bad0face8d75");
        public const string ProcessingOrderStatusName = "Processing";

        public static Guid CompleteOrderStatusTaxonomyId { get; } = Guid.Parse("69773fe3-80f8-436f-99e0-cfd45cafe723");
        public const string CompleteOrderStatusName = "Complete";

        public static Guid ShippedStatusTaxonomyId { get; } = Guid.Parse("0edbe400-37a4-4cee-8223-1099ee570187");
        public const string ShippedOrderStatusName = "Shipped";

        public static Guid ClosedStatusTaxonomyId { get; } = Guid.Parse("cc9817cf-d328-427a-835e-092e3547f88f");
        public const string ClosedOrderStatusName = "Closed";

        public static Guid CanceledStatusTaxonomyId { get; } = Guid.Parse("00b820f6-e5c1-44a2-972d-ada08ae34756");
        public const string CanceledStatusName = "Canceled";
    }
}
