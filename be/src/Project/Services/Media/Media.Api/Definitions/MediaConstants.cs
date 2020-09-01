using System;

namespace Media.Api.Definitions
{
    public static class MediaConstants
    {
        public struct General
        {
            public static readonly string ContainerName = "giuru";
        }

        public struct HeroSliderItems
        {
            public static readonly Guid LivingRoomMediaId = Guid.Parse("c6c96ab8-a81f-446f-adc2-d375479ece98");
            public static readonly Guid LivingRoomMediaVersionId = Guid.Parse("c5fb5a7c-6cc0-4cde-8998-ff1d397f1874");
            public static readonly string LivingRoomMediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/LivingRoom.jpg";

            public static readonly Guid BedroomMediaId = Guid.Parse("01bfe732-cfca-4cdf-a740-9f8e1ba0a537");
            public static readonly Guid BedroomMediaVersionId = Guid.Parse("f6030dbf-3eaf-4e2a-b2dd-6ee3c53b67ff");
            public static readonly string BedroomMediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/Bedroom.jpg";

            public static readonly Guid KidsRoomMediaId = Guid.Parse("3c57aa8e-c54a-4571-8112-936d75331657");
            public static readonly Guid KidsRoomMediaVersionId = Guid.Parse("f3dc4359-96b4-46f6-aa66-d8f3c3d1c808");
            public static readonly string KidsRoomMediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/KidsRoom.jpg";
        }

        public struct Categories
        {
            public static readonly Guid CouchesMediaId = Guid.Parse("bdd7240c-e05f-42f0-9310-ddd4ed37ca18");
            public static readonly Guid CouchesMediaVersionId = Guid.Parse("fcd4b61f-3356-4e51-a580-7b6d2968884e");
            public static readonly string CouchesMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/Couches.jpg";

            public static readonly Guid SectionalsMediaId = Guid.Parse("f9271cb1-d3f8-4b2b-bb77-2648b25ff347");
            public static readonly Guid SectionalsMediaVersionId = Guid.Parse("b35c70ab-16cf-4b55-aaa0-52747056e059");
            public static readonly string SectionalsMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/Sectionals.jpg";

            public static readonly Guid CoffeeTablesMediaId = Guid.Parse("22ee068c-cb80-483e-908c-9abbeb9b83ad");
            public static readonly Guid CoffeeTablesMediaVersionId = Guid.Parse("d66ae4f5-9772-4f47-8f12-094979185111");
            public static readonly string CoffeeTablesMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/CoffeeTables.jpg";

            public static readonly Guid TvStandsMediaId = Guid.Parse("80167673-c46e-4a63-a242-8042e9fdc6aa");
            public static readonly Guid TvStandsMediaVersionId = Guid.Parse("06f9dea9-b7b9-4614-8b44-c68a47cf40a9");
            public static readonly string TvStandsMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/TvTables.jpg";

            public static readonly Guid WallUnitsMediaId = Guid.Parse("8120e8a6-f364-4611-b133-040fd163ecdd");
            public static readonly Guid WallUnitsMediaVersionId = Guid.Parse("70ffc1f1-6b80-4527-8236-f233326d2455");
            public static readonly string WallUnitsMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/WallUnits.jpg";

            public static readonly Guid ChairsMediaId = Guid.Parse("d0d611c1-67bd-46ef-9133-382d161c09e5");
            public static readonly Guid ChairsMediaVersionId = Guid.Parse("3958f1df-aca5-47d7-91ee-6319bd693ef5");
            public static readonly string ChairsMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/Armchairs.jpg";

            public static readonly Guid PoufsMediaId = Guid.Parse("27a91dfe-139b-4c36-a716-9dacc0c685c9");
            public static readonly Guid PoufsMediaVersionId = Guid.Parse("e6d8ede2-a786-413c-887f-b33bc3f5fa41");
            public static readonly string PoufsMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/Poufs.jpg";

            public static readonly Guid BedsMediaId = Guid.Parse("1aacaa2a-0719-45e6-9bcd-0f5ed2bbe4b8");
            public static readonly Guid BedsMediaVersionId = Guid.Parse("c6a71890-c424-40b3-adad-6d98388566b3");
            public static readonly string BedsMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/Beds.jpg";

            public static readonly Guid WardrobesMediaId = Guid.Parse("f593f89d-354f-4e00-9cb5-50a32462cf9e");
            public static readonly Guid WardrobesMediaVersionId = Guid.Parse("c14b7541-8f41-4040-9c3c-0a04743eefeb");
            public static readonly string WardrobesMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/Wardrobes.jpg";

            public static readonly Guid ChestsMediaId = Guid.Parse("00d64a65-0b50-4869-bd9f-38ae2e9dc417");
            public static readonly Guid ChestsMediaVersionId = Guid.Parse("63c12bd8-8044-4cc5-acd5-409d14ec1779");
            public static readonly string ChestsMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/Chests.jpg";

            public static readonly Guid MattressesMediaId = Guid.Parse("ce5f8282-e6a0-44c4-bb1e-e186f2f8c1da");
            public static readonly Guid MattressesMediaVersionId = Guid.Parse("23beeaec-8a7e-4397-83a6-7bf450b9e491");
            public static readonly string MattressesMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/BoxSprings.jpg";

            public static readonly Guid DiningTablesSeatingMediaId = Guid.Parse("54e9fe96-8852-4677-a409-4dd154e56822");
            public static readonly Guid DiningTablesSeatingMediaVersionId = Guid.Parse("1131e368-eb09-4ae2-875a-78160c09b517");
            public static readonly string DiningTablesSeatingMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/KitchenTables.jpg";

            public static readonly Guid BathroomVanitiesCabinetsMediaId = Guid.Parse("36b0b2f3-cc5c-455d-bac3-3679238e094a");
            public static readonly Guid BathroomVanitiesCabinetsMediaVersionId = Guid.Parse("0735f4f5-9f3f-41e0-b0b1-c946a852141d");
            public static readonly string BathroomVanitiesCabinetsMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/BathroomCabinets.jpg";

            public static readonly Guid KidsBedsMediaId = Guid.Parse("8f274241-0c37-43b0-b24a-a97fddfaa8b2");
            public static readonly Guid KidsBedsMediaVersionId = Guid.Parse("5b9a9b02-d76a-4180-b43d-6013f64512a8");
            public static readonly string KidsBedsMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/KidsBeds.jpg";

            public static readonly Guid KidsBunkBedsMediaId = Guid.Parse("368c1c95-cb0c-45aa-a8ed-cffa1e0aa33e");
            public static readonly Guid KidsBunkBedsMediaVersionId = Guid.Parse("adb7edaa-050d-4dce-95cc-3ab60504d2a3");
            public static readonly string KidsBunkBedsMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/BunkBeds.jpg";

            public static readonly Guid KidsDesksMediaId = Guid.Parse("87ddb36d-080c-429f-8dba-b38db9ae8e8b");
            public static readonly Guid KidsDesksMediaVersionId = Guid.Parse("9c89dcc7-3902-4d1c-8bcd-088db9e85120");
            public static readonly string KidsDesksMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/KidsDesks.jpg";

        }
    }
}
