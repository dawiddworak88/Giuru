using System;

namespace Giuru.IntegrationTests.Definitions
{
    public struct Clients
    {
        public static readonly Guid Id = Guid.Parse("7d4fe733-baa2-4c70-83a5-2e4ff1b5274b");
        public const string Name = "Giuru-Tests";
        public const string UpdatedName = "Giuru-Updated";
        public const string Email = "giuru@tests.com";
        public const string Language = "en";
        public static readonly Guid OrganisationId = Guid.Parse("09affcc9-1665-45d6-919f-3d2026106ba1");
    }
}
