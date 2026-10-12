namespace Foundation.Extensions.Definitions
{
    public static class ClaimsEnrichmentConstants
    {
        public static readonly string CacheKey = "Claims";

        /// <summary>
        /// Prefix of the enriched claims of a buyer. The cache key also names the buyer's organisation, because the
        /// client a signed-in user prices for is the client of their organisation, which a team member does not
        /// share an email address with. The prefix differs from <see cref="CacheKey"/> so that the old email-only
        /// entries are never read as organisation-scoped ones.
        /// </summary>
        public static readonly string ClientClaimsCacheKey = "ClientClaims";

        public static string GetClientClaimsCacheKey(string email, System.Guid? organisationId)
        {
            return $"{ClientClaimsCacheKey}-{organisationId?.ToString("N") ?? "none"}-{email}";
        }
    }
}
