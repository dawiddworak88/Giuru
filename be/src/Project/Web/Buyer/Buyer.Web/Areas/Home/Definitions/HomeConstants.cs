using System;

namespace Buyer.Web.Areas.Home.Definitions
{
    public static class HomeConstants
    {
        public struct News
        {
            public static readonly Guid NewsId = Guid.Parse("5632bec3-9087-48e3-8406-f5272f006506");
        }

        public struct ContentGrid
        {
            public static readonly int ContentItemImageMaxWidth = 780;
            public static readonly int ContentItemImageMaxHeight = 780;
        }

        public struct Categories
        {
            public static readonly int FirstLevel = 1;
            public static readonly int SecondLevel = 2;
        }

        public struct Novelties
        {
            public static readonly Guid NoveltiesId = Guid.Parse("3c6726ae-10ed-49f6-be59-66dcd782ee9f");
        }
    }
}
