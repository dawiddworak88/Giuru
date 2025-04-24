using System;

namespace Giuru.IntegrationTests.Definitions
{
    public struct Inventories
    {
        public static readonly Guid WarehouseId = Guid.Parse("1b4a61fb-cdda-45b2-a4d6-92a27acdf833");

        public struct Quantities
        {
            public const int Quantity = 2;
            public const int AvailableQuantity = 2;
        }
    }
}
