using System;

namespace Giuru.IntegrationTests.Definitions
{
    public struct Products
    {
        // Never created in the catalog. The basket tests send it with this made-up id, which only works while the SKU
        // resolves to no product: a catalogued SKU with another id is refused as an inconsistent basket line.
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

        public struct Nela
        {
            public const string Name = "Nela";
            public const string Sku = "NEL_01";
            public const string UpdatedName = "Nela 180x200";
            public static readonly Guid CategoryId = Guid.Parse("1b4a61fb-cdda-45b2-a4d6-92a27acdf833");
            public const bool IsPublished = true;
            public const string Ean = "6978494041190";
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
    }
}
