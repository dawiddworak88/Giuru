using System;

namespace Download.Api.Definitions
{
    public static class DownloadConstants
    {
        public struct Categories
        {
            public static readonly Guid MarketingCategory = Guid.Parse("7908f718-8d2e-4776-9fa1-1b44fbcd2717");
            public static readonly Guid BannersCategory = Guid.Parse("f4b54ecb-cd3e-4731-bd5b-0e888d852f5e");
            public static readonly Guid GifsCategory = Guid.Parse("7ca59ac4-b8fb-465a-84c4-cae8b0a6ea70");
            public static readonly Guid CollectionsCategory = Guid.Parse("1488e313-c528-4e6d-9246-63a754136723");
            public static readonly Guid ColorsOfProductsCategory = Guid.Parse("dbb59752-4fe7-45c9-982c-ae3d26a25435");
            public static readonly Guid TechnicalCategory = Guid.Parse("391070b0-7dcf-4010-ac7d-c5f74aed90f2");
            public static readonly Guid TechnicalSheetsCategory = Guid.Parse("b05e3dd2-5c68-455e-9de4-45fda60f84b7");
        }

        public struct Downloads
        {
            public static readonly Guid CollectionDownloads = Guid.Parse("3f51b5f9-8e32-4169-ad8b-71569fcd32a9");
            public static readonly Guid MarketingDownloads = Guid.Parse("bb28e8d4-643d-4541-a87a-277caa8b4381");
            public static readonly Guid TechnicalDownloads = Guid.Parse("365b0687-6f26-49a6-b2fe-d21a6a2f9fe5");
        }
    }
}
