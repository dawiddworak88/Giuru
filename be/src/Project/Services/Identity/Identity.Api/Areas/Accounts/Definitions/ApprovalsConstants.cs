using System;

namespace Identity.Api.Areas.Accounts.Definitions
{
    public static class ApprovalsConstants
    {
        public struct Marketing
        {
            public readonly static Guid InformationByEmail = Guid.Parse("7addb70a-0942-488d-ba27-afa5b570ede5");
            public readonly static Guid InformationBySms = Guid.Parse("d34ec3d3-bb88-4de5-9391-d9d29609551a");
        }
    }
}
