using System;

namespace Buyer.Web.Areas.Home.Definitions
{
    public static class HomeConstants
    {
        public struct News
        {
            public static readonly int DefaultPageIndex = 1;
            public static readonly int DefaultPageSize = 6;
            public static readonly string DefaultSearchTerm = "B2B";
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
