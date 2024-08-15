using System;

namespace Giuru.IntegrationTests.Definitions
{
    public struct Products
    {
        public struct Lamica
        {
            public const string Name = "Lamica";
            public const string UpdatedName = "Lamica 180x200";
            public static readonly Guid CategoryId = Guid.Parse("1b4a61fb-cdda-45b2-a4d6-92a27acdf833");
            public const bool IsPublished = true;
            public const string Ean = "6978494041189";
        }
    }
}
