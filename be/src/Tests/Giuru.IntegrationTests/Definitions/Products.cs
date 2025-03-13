using System;

namespace Giuru.IntegrationTests.Definitions
{
    public struct Products
    {
        public struct Lamica
        {
            public static readonly Guid Id = Guid.Parse("9cccc453-8475-4ef8-a00d-2743dcd72964");
            public const string Name = "Lamica";
            public const string Sku = "LAM_01";
            public const string UpdatedName = "Lamica 180x200";
            public static readonly Guid CategoryId = Guid.Parse("1b4a61fb-cdda-45b2-a4d6-92a27acdf833");
            public const bool IsPublished = true;
            public const string Ean = "6978494041189";
        }

        public struct Anton
        {
            public const string Name = "Anton";
            public const string Sku = "AN_01";
            public const string UpdatedName = "Anton";
            public static readonly Guid CategoryId = Guid.Parse("1b4a61fb-cdda-45b2-a4d6-92a27acdf833");
            public const bool IsPublished = true;
            public const string Ean = "6978494041191";
        }

        public struct Aga
        {
            public const string Name = "Aga";
            public const string Sku = "AG_01";
            public const string UpdatedName = "Aga";
            public static readonly Guid CategoryId = Guid.Parse("1b4a61fb-cdda-45b2-a4d6-92a27acdf833");
            public const bool IsPublished = true;
            public const string Ean = "6978494041192";
        }

        public struct Quantities
        {
            public const int Quantity = 2;
            public const int AvailableQuantity = 2;
        }
    }
}
