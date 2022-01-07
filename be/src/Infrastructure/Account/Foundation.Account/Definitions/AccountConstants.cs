namespace Foundation.Account.Definitions
{
    public static class AccountConstants
    {
        public static readonly string HttpsScheme = "https";
        public static readonly string HttpScheme = "http";
        public static readonly string OrganisationIdClaim = "OrganisationId";
        public static readonly int DefaultTokenLifetimeInDays = 1;
        public static readonly int DefaultTokenLifetimeInSeconds = 1 * 24 * 60 * 60;
    }
}
