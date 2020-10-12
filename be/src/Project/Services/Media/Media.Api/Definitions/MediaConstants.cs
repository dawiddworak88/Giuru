using System;

namespace Media.Api.Definitions
{
    public static class MediaConstants
    {
        public struct General
        {
            public static readonly string ContainerName = "giuru";
        }

        public struct Headers
        {
            public static readonly Guid LogoMediaId = Guid.Parse("85b14b09-856d-4fd0-8af4-7c077953b214");
            public static readonly Guid LogoMediaVersionId = Guid.Parse("615434fe-5a08-432d-a2c9-5ac4e0830713");
            public static readonly string LogoMediaUrl = "./Infrastructure/Media/Seeds/Images/Headers/logo.png";

            public static readonly Guid FaviconMediaId = Guid.Parse("0b8ed469-e84c-455b-b616-12a60d38da1c");
            public static readonly Guid FaviconMediaVersionId = Guid.Parse("5a9a1483-89db-4f70-b77f-c45c12b3001d");
            public static readonly string FaviconMediaUrl = "./Infrastructure/Media/Seeds/Images/Headers/favicon.png";
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

            public static readonly Guid MattressesMediaId = Guid.Parse("ce5f8282-e6a0-44c4-bb1e-e186f2f8c1da");
            public static readonly Guid MattressesMediaVersionId = Guid.Parse("23beeaec-8a7e-4397-83a6-7bf450b9e491");
            public static readonly string MattressesMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/BoxSprings.jpg";
        }
    }
}
