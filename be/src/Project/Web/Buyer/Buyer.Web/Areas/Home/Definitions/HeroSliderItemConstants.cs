using System;

namespace Buyer.Web.Areas.Home.Definitions
{
    public static class HeroSliderItemConstants
    {
        public struct Media
        {
            public static readonly Guid BoxspringsMediaId = Guid.Parse("3d206600-4d0d-4bb0-880a-90ba85bbaeb5");
            public static readonly Guid Boxsprings1600x400MediaId = Guid.Parse("126332f9-37d1-4ded-b05a-65c488350e67");
            public static readonly Guid Boxsprings1024x400MediaId = Guid.Parse("3053043d-d667-40b7-bd8c-357e9456d5e5");
            public static readonly Guid Boxsprings414x286MediaId = Guid.Parse("57c78fa6-a09d-4ce3-bd6a-e000552de03b");

            public static readonly Guid ChairsMediaId = Guid.Parse("09e85fba-f63a-45e2-87ab-12e3b78ed650");
            public static readonly Guid Chairs1600x400MediaId = Guid.Parse("2733247e-5c04-465e-863f-bc96663f2f5f");
            public static readonly Guid Chairs1024x400MediaId = Guid.Parse("a6c5d04f-a437-4065-89fb-73c8e84c133e");
            public static readonly Guid Chairs414x286MediaId = Guid.Parse("cf1dae4a-d093-4f03-aa8b-adc739dd57f9");

            public static readonly Guid CornersMediaId = Guid.Parse("19221400-3812-49b6-a8d7-fb8bc8223919");
            public static readonly Guid Corners1600x400MediaId = Guid.Parse("854e6468-1806-497d-a13b-197cc87f6664");
            public static readonly Guid Corners1024x400MediaId = Guid.Parse("3b15c40e-c907-4c0f-b026-af35b5824167");
            public static readonly Guid Corners414x286MediaId = Guid.Parse("a4029144-11b1-4031-8a69-23d288ef84f7");

            public static readonly Guid SetsMediaId = Guid.Parse("4c299e20-c8c5-47e1-bbe3-c308e8bd4478");
            public static readonly Guid Sets1600x400MediaId = Guid.Parse("7eb13893-07e6-44f0-a652-986b96713911");
            public static readonly Guid Sets1024x400MediaId = Guid.Parse("0731bdda-9f91-4616-ac88-3e54be4d1659");
            public static readonly Guid Sets414x286MediaId = Guid.Parse("c44e743d-1073-43ac-8be2-09c6e29c0318");
        }

        public struct Categories
        {
            public struct Sectionals 
            {
                public static readonly Guid Id = Guid.Parse("4d54153b-10a6-4bf7-9a44-a38759d8be53");
            }

            public struct Beds
            {
                public static readonly Guid Id = Guid.Parse("1b4a61fb-cdda-45b2-a4d6-92a27acdf833");
            }

            public struct Chairs
            {
                public static readonly Guid Id = Guid.Parse("b89c7c6c-a4f2-442b-8813-51c39291aa4e");
            }

            public struct Sets
            {
                public static readonly Guid Id = Guid.Parse("c8fc3b79-c17f-4ac8-99a8-e02619ed17ec");
            }
        }
    }
}
