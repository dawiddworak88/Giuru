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

        /// <summary>
        /// Prefix of the current version of an organisation's cached client claims. The primary contact and every team
        /// member cache their claims under their own email, so an edit of the client cannot remove them one by one: it
        /// replaces the version instead, and the entries of every member of the organisation stop being read at once.
        /// </summary>
        public static readonly string ClientClaimsVersionCacheKey = "ClientClaimsVersion";

        public static string GetClientClaimsCacheKey(string email, System.Guid? organisationId, string version)
        {
            return $"{ClientClaimsCacheKey}-{FormatOrganisationId(organisationId)}-{version}-{email}";
        }

        public static string GetClientClaimsVersionCacheKey(System.Guid? organisationId)
        {
            return $"{ClientClaimsVersionCacheKey}-{FormatOrganisationId(organisationId)}";
        }

        private static string FormatOrganisationId(System.Guid? organisationId)
        {
            return organisationId?.ToString("N") ?? "none";
        }
    }
}
