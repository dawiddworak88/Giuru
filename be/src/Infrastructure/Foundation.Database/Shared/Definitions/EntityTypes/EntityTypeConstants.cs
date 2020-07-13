using System;

namespace Foundation.Database.Shared.Definitions.EntityTypes
{
    public static class EntityTypeConstants
    {
        public static Guid ProductId { get; } = Guid.Parse("62841BBB-7754-4197-DE80-08D805A35B27");
        public const string Product = "Product";

        public static Guid OrderId { get; } = Guid.Parse("8b3527a7-c833-405f-9883-eba6540b9576");
        public const string Order = "Order";

        public static Guid OrderItemId { get; } = Guid.Parse("e8e79209-deca-4a3b-8cf6-83d3f5e47753");
        public const string OrderItem = "OrderItem";

        public static Guid ClientId { get; } = Guid.Parse("21fd2044-0192-4a25-bebb-43db9ae31db5");
        public const string Client = "Client";
    }
}
