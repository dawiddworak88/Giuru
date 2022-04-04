using System;

namespace News.Api.Definitions
{
    public static class NewsConstants
    {
        public struct Categories
        {
            public static readonly Guid InformationCategory = Guid.Parse("7908f718-8d2e-4776-9fa1-1b44fbcd2717");
            public static readonly Guid EventsCategory = Guid.Parse("f4b54ecb-cd3e-4731-bd5b-0e888d852f5e");
            public static readonly Guid BusinessCategory = Guid.Parse("7ca59ac4-b8fb-465a-84c4-cae8b0a6ea70");
            public static readonly Guid FairCategory = Guid.Parse("1488e313-c528-4e6d-9246-63a754136723");
            public static readonly Guid ExhibitionCategory = Guid.Parse("dbb59752-4fe7-45c9-982c-ae3d26a25435");
            public static readonly Guid SalesCategory = Guid.Parse("391070b0-7dcf-4010-ac7d-c5f74aed90f2");
            public static readonly Guid CooperationCategory = Guid.Parse("b05e3dd2-5c68-455e-9de4-45fda60f84b7");
        }
    }
}
