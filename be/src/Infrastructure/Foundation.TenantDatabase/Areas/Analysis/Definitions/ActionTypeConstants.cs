using System;

namespace Foundation.TenantDatabase.Areas.Analysis.Definitions
{
    public static class ActionTypeConstants
    {
        public static Guid NewOrderId { get; } = Guid.Parse("c536137f-ba09-4bda-92d8-f0da2c1e7dfc");
        public const string NewOrder = "NewOrder";

        public static Guid NewOrderItemId { get; } = Guid.Parse("c536137f-ba09-4bda-92d8-f0da2c1e7dfc");
        public const string NewOrderItem = "NewOrderItem";

        public static Guid ChangeOrderItemStatusId { get; } = Guid.Parse("930ec958-1183-4682-be78-0e1e99a813e6");
        public const string ChangeOrderItemStatus = "ChangeOrderItemStatus";

        public static Guid NewComplaintId { get; } = Guid.Parse("47153047-e1bd-4169-9388-bd08de5702f4");
        public const string NewComplaint = "NewComplaint";
    }
}
