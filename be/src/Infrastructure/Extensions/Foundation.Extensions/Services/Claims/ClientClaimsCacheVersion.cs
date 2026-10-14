using Foundation.Extensions.Definitions;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace Foundation.Extensions.Services.Claims
{
    /// <summary>
    /// The version an organisation's client claims are cached under (see
    /// <see cref="ClaimsEnrichmentConstants.ClientClaimsVersionCacheKey"/>). A version is a random token rather than a
    /// counter: the distributed cache has no atomic increment, and a token that is never reused needs none.
    /// </summary>
    public static class ClientClaimsCacheVersion
    {
        // Much longer than the claims themselves are cached for, so an active organisation keeps its version. Losing it
        // early is harmless: a new version is started, and the claims are looked up again.
        private static readonly DistributedCacheEntryOptions VersionEntryOptions = new()
        {
            SlidingExpiration = TimeSpan.FromDays(1)
        };

        /// <summary>
        /// The current version, started if there is none. A missing version - never set, or evicted - never falls back
        /// to a fixed value, so entries cached under an earlier version cannot be read again. Read it before the client
        /// is looked up: claims looked up before an invalidation are then cached under the version it replaced.
        /// </summary>
        public static async Task<string> GetOrStartAsync(IDistributedCache cache, Guid? organisationId)
        {
            var versionKey = ClaimsEnrichmentConstants.GetClientClaimsVersionCacheKey(organisationId);
            var version = await cache.GetStringAsync(versionKey);

            if (string.IsNullOrWhiteSpace(version))
            {
                version = await StartAsync(cache, versionKey);
            }

            return version;
        }

        /// <summary>Replaces the version, so the claims every member of the organisation has cached are no longer read.</summary>
        public static Task RenewAsync(IDistributedCache cache, Guid? organisationId)
        {
            return StartAsync(cache, ClaimsEnrichmentConstants.GetClientClaimsVersionCacheKey(organisationId));
        }

        private static async Task<string> StartAsync(IDistributedCache cache, string versionKey)
        {
            var version = Guid.NewGuid().ToString("N");

            await cache.SetStringAsync(versionKey, version, VersionEntryOptions);

            return version;
        }
    }
}
