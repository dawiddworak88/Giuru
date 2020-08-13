using System;

namespace Catalog.Api.Infrastructure.Categories.Definitions
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

                public static class LivingRoom
                {
                    public static readonly Guid SofasId = Guid.Parse("45046646-e0c5-4a7d-a087-de91a290912d");
                    public static readonly Guid SectionalsId = Guid.Parse("4d54153b-10a6-4bf7-9a44-a38759d8be53");
                    public static readonly Guid CoffeeTablesId = Guid.Parse("9095f312-ebb8-4525-823a-42f30bf84e26");
                    public static readonly Guid TvStandsId = Guid.Parse("528fc588-b4ee-44e2-a259-b5a3579cb220");
                    public static readonly Guid WallUnitsId = Guid.Parse("4b2b8143-fb58-4330-8e39-928afd5c5286");
                    public static readonly Guid ChairsId = Guid.Parse("b89c7c6c-a4f2-442b-8813-51c39291aa4e");
                    public static readonly Guid PoufsId = Guid.Parse("8d011649-e113-42f4-a600-5ada832b859b");
                    public static readonly Guid LivingRoomSetsId = Guid.Parse("c8fc3b79-c17f-4ac8-99a8-e02619ed17ec");
                }

                public static class Bedroom
                {
                    public static readonly Guid BedsId = Guid.Parse("1b4a61fb-cdda-45b2-a4d6-92a27acdf833");
                    public static readonly Guid WardrobesId = Guid.Parse("2cf5e97c-49ae-4679-90b9-734dfa2751c4");
                    public static readonly Guid ChestsId = Guid.Parse("505f8cee-abb8-4915-aad9-0964d6560385");
                    public static readonly Guid MattressesId = Guid.Parse("d396d9e6-fdbc-409f-a3f1-765101f97672");
                }

                public static class KitchenDiningRoom
                {
                    public static readonly Guid DiningTablesSeatingId = Guid.Parse("32655256-e121-4749-a78e-00578c4b45e4");
                    public static readonly Guid KitchenCabinetsId = Guid.Parse("2a8f1365-ea7b-4f6f-8e81-0a7903610d88");
                    public static readonly Guid ChinaCabinetsId = Guid.Parse("1cd61cd8-9543-4151-9702-3094a1dbca9f");
                }

                public static class Bathroom
                {
                    public static readonly Guid BathroomVanitiesCabinetsId = Guid.Parse("92bc8fca-2edb-4d68-9940-cd8c0816f95a");
                }

                public static class KidsFurniture
                {
                    public static readonly Guid KidsBedsId = Guid.Parse("ed0a4b80-75c1-4cc8-b6be-1ac21fcb7889");
                    public static readonly Guid KidsBunkBedsId = Guid.Parse("b2349e95-bf76-45f6-9ace-d4a002d96e12");
                    public static readonly Guid KidsTablesId = Guid.Parse("71597da0-7941-49c5-bcbe-1c551779ed0a");
                    public static readonly Guid KidsDesksId = Guid.Parse("edb2081c-9bc3-4fc9-b6b4-c7fe9e223f75");
                }
            }
        }
    }
}
