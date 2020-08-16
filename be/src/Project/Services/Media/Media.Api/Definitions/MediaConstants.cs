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
            public static Guid SectionalsMediaId = Guid.Parse("f9271cb1-d3f8-4b2b-bb77-2648b25ff347");
            public static Guid SectionalsMediaVersionId = Guid.Parse("b35c70ab-16cf-4b55-aaa0-52747056e059");
            public const string SectionalsMediaUrl = "https://eltap.pl/upload/kategorie/62641_s.jpg";
        }
    }
}
