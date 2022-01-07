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
            public static readonly Guid BoxspringsMediaId = Guid.Parse("3d206600-4d0d-4bb0-880a-90ba85bbaeb5");
            public static readonly Guid BoxspringsMediaVersionId = Guid.Parse("9896ad51-e2e3-42f7-9c1e-fbfba9de117e");
            public static readonly string BoxspringsMediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/Boxsprings.jpg";

            public static readonly Guid Boxsprings1600x400MediaId = Guid.Parse("126332f9-37d1-4ded-b05a-65c488350e67");
            public static readonly Guid Boxsprings1600x400MediaVersionId = Guid.Parse("5f731b13-ebfd-4efd-b7e7-83988472c2e4");
            public static readonly string Boxsprings1600x400MediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/Boxsprings_1600x400.jpg";

            public static readonly Guid Boxsprings1024x400MediaId = Guid.Parse("3053043d-d667-40b7-bd8c-357e9456d5e5");
            public static readonly Guid Boxsprings1024x400MediaVersionId = Guid.Parse("bd6d68b4-f6ea-4946-a6d4-aa500446ae8e");
            public static readonly string Boxsprings1024x400MediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/Boxsprings_1024x400.jpg";

            public static readonly Guid Boxsprings414x286MediaId = Guid.Parse("57c78fa6-a09d-4ce3-bd6a-e000552de03b");
            public static readonly Guid Boxsprings414x286MediaVersionId = Guid.Parse("0f0ea16a-88b1-4b36-b3dc-326bdb9b6bf4");
            public static readonly string Boxsprings414x286MediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/Boxsprings_414x286.jpg";

            public static readonly Guid ChairsMediaId = Guid.Parse("09e85fba-f63a-45e2-87ab-12e3b78ed650");
            public static readonly Guid ChairsMediaVersionId = Guid.Parse("e8c3b904-7dc2-43ba-921e-57bed71b471b");
            public static readonly string ChairsMediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/Chairs.jpg";

            public static readonly Guid Chairs1600x400MediaId = Guid.Parse("2733247e-5c04-465e-863f-bc96663f2f5f");
            public static readonly Guid Chairs1600x400MediaVersionId = Guid.Parse("c4b2bfdb-6b16-41d4-9013-b4bc40847ca1");
            public static readonly string Chairs1600x400MediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/Chairs_1600x400.jpg";

            public static readonly Guid Chairs1024x400MediaId = Guid.Parse("a6c5d04f-a437-4065-89fb-73c8e84c133e");
            public static readonly Guid Chairs1024x400MediaVersionId = Guid.Parse("e456f423-f615-4ab6-ac8e-bff482d75e94");
            public static readonly string Chairs1024x400MediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/Chairs_1024x400.jpg";

            public static readonly Guid Chairs414x286MediaId = Guid.Parse("cf1dae4a-d093-4f03-aa8b-adc739dd57f9");
            public static readonly Guid Chairs414x286MediaVersionId = Guid.Parse("2e4d4644-7005-461c-a35c-ebcdaee53522");
            public static readonly string Chairs414x286MediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/Chairs_414x286.jpg";

            public static readonly Guid CornersMediaId = Guid.Parse("19221400-3812-49b6-a8d7-fb8bc8223919");
            public static readonly Guid CornersMediaVersionId = Guid.Parse("16ae37a9-bff4-41e1-8813-84bb65568028");
            public static readonly string CornersMediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/Corners.jpg";

            public static readonly Guid Corners1600x400MediaId = Guid.Parse("854e6468-1806-497d-a13b-197cc87f6664");
            public static readonly Guid Corners1600x400MediaVersionId = Guid.Parse("0e522f44-3ca7-4e4c-b6d2-d141ab8bad2f");
            public static readonly string Corners1600x400MediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/Corners_1600x400.jpg";

            public static readonly Guid Corners1024x400MediaId = Guid.Parse("3b15c40e-c907-4c0f-b026-af35b5824167");
            public static readonly Guid Corners1024x400MediaVersionId = Guid.Parse("8ccc10c2-52f8-4de1-b486-16eae8c1b71d");
            public static readonly string Corners1024x400MediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/Corners_1024x400.jpg";

            public static readonly Guid Corners414x286MediaId = Guid.Parse("a4029144-11b1-4031-8a69-23d288ef84f7");
            public static readonly Guid Corners414x286MediaVersionId = Guid.Parse("03234f10-c21c-4558-a187-64a4b9443f02");
            public static readonly string Corners414x286MediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/Corners_414x286.jpg";

            public static readonly Guid SetsMediaId = Guid.Parse("4c299e20-c8c5-47e1-bbe3-c308e8bd4478");
            public static readonly Guid SetsMediaVersionId = Guid.Parse("f2bb7931-13c6-4018-a4cb-a3570c7c6545");
            public static readonly string SetsMediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/Sets.jpg";

            public static readonly Guid Sets1600x400MediaId = Guid.Parse("7eb13893-07e6-44f0-a652-986b96713911");
            public static readonly Guid Sets1600x400MediaVersionId = Guid.Parse("84164434-f709-42d6-8aa5-6fd8f352e1c6");
            public static readonly string Sets1600x400MediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/Sets_1600x400.jpg";

            public static readonly Guid Sets1024x400MediaId = Guid.Parse("0731bdda-9f91-4616-ac88-3e54be4d1659");
            public static readonly Guid Sets1024x400MediaVersionId = Guid.Parse("35887bad-7ba4-44ae-a550-3e9b2cf27ae5");
            public static readonly string Sets1024x400MediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/Sets_1024x400.jpg";

            public static readonly Guid Sets414x286MediaId = Guid.Parse("c44e743d-1073-43ac-8be2-09c6e29c0318");
            public static readonly Guid Sets414x286MediaVersionId = Guid.Parse("0677422f-4955-48c3-b41f-cabc8875596d");
            public static readonly string Sets414x286MediaUrl = "./Infrastructure/Media/Seeds/Images/HeroSliderItems/Sets_414x286.jpg";
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

            public static readonly Guid SetsMediaId = Guid.Parse("f2aeeb0d-455c-422b-987b-6ded4295bd74");
            public static readonly Guid SetsMediaVersionId = Guid.Parse("62f2c58e-f461-4347-9476-41de6d170e50");
            public static readonly string SetsMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/Sets.jpg";

            public static readonly Guid BedsMediaId = Guid.Parse("1aacaa2a-0719-45e6-9bcd-0f5ed2bbe4b8");
            public static readonly Guid BedsMediaVersionId = Guid.Parse("c6a71890-c424-40b3-adad-6d98388566b3");
            public static readonly string BedsMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/Beds.jpg";

            public static readonly Guid MattressesMediaId = Guid.Parse("ce5f8282-e6a0-44c4-bb1e-e186f2f8c1da");
            public static readonly Guid MattressesMediaVersionId = Guid.Parse("23beeaec-8a7e-4397-83a6-7bf450b9e491");
            public static readonly string MattressesMediaUrl = "./Infrastructure/Media/Seeds/Images/Categories/Mattresses.jpg";
        }

        public struct ImageConversion
        {
            public static readonly int ImageQuality = 75;
        }

        public struct MimeTypes
        {
            public const string Jpeg = "image/jpeg";
            public const string Png = "image/png";
            public const string Svg = "image/svg+xml";
        }
    }
}
