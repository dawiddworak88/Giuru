using System;

namespace Media.Api.Definitions
{
    public static class MediaConstants
    {
        public struct General
        {
            public const string ContainerName = "giuru";
        }

        public struct Categories
        {
            public static Guid CouchesMediaId = Guid.Parse("bdd7240c-e05f-42f0-9310-ddd4ed37ca18");
            public static Guid CouchesMediaVersionId = Guid.Parse("fcd4b61f-3356-4e51-a580-7b6d2968884e");
            public const string CouchesMediaUrl = "/Images/Categories/Couches.jpg";

            public static Guid SectionalsMediaId = Guid.Parse("f9271cb1-d3f8-4b2b-bb77-2648b25ff347");
            public static Guid SectionalsMediaVersionId = Guid.Parse("b35c70ab-16cf-4b55-aaa0-52747056e059");
            public const string SectionalsMediaUrl = "/Images/Categories/Sectionals.jpg";
        }
    }
}
