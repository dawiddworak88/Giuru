using System;

namespace Foundation.Database.Areas.Products.Definitions
{
    public static class Constants
    {
        public static class CategoryGuids
        {
            public static readonly Guid FurnitureId = Guid.Parse("e626a139-7068-4de0-abfe-1b970975f1d2");

            public static class Furniture
            {
                public static readonly Guid LivingRoomFurnitureId = Guid.Parse("5de3cf93-e4de-492e-98a0-5b65fb41cca5");
                public static readonly Guid BedroomFurnitureId = Guid.Parse("e1ec2500-aff4-4c42-926b-f155ab5ceed3");
                public static readonly Guid KichtenDiningFurnitureId = Guid.Parse("90400643-8976-44c8-b61f-f6c700ed45bf");
                public static readonly Guid AccentFurnitureId = Guid.Parse("b72acb48-4653-4ed7-8766-d4aa0d6e29c7");
                public static readonly Guid OfficeFurnitureId = Guid.Parse("9c9f3453-e00e-4532-88d8-e96db14d5982");
                public static readonly Guid EntryFurnitureId = Guid.Parse("eb247a44-5513-40ee-84ef-5d15eec29545");
                public static readonly Guid OutdoorFurnitureId = Guid.Parse("b65ed50e-8f51-4d63-be41-7c04af91602f");
                public static readonly Guid BathroomFurnitureId = Guid.Parse("5853a764-8b27-4b34-84e4-23ee79451d0d");
                public static readonly Guid KidsFurnitureId = Guid.Parse("21c97e53-e36a-4c35-b8c2-a50aaad517e7");
            }
        }
    }
}
