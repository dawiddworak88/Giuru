using System;

namespace Buyer.Web.Areas.Home.Definitions
{
    public static class HeroSliderItemConstants
    {
        public struct Media
        {
            public static readonly Guid LivingRoomMediaId = Guid.Parse("c6c96ab8-a81f-446f-adc2-d375479ece98");
            public static readonly Guid BedroomMediaId = Guid.Parse("01bfe732-cfca-4cdf-a740-9f8e1ba0a537");
            public static readonly Guid KidsRoomMediaId = Guid.Parse("3c57aa8e-c54a-4571-8112-936d75331657");
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
        }
    }
}
