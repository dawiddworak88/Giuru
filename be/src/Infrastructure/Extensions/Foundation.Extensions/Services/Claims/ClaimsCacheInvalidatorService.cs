using Foundation.Extensions.Definitions;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace Foundation.Extensions.Services.Claims
{
    public class ClaimsCacheInvalidatorService : IClaimsCacheInvalidatorService
    {
        private readonly IDistributedCache _cache;

        public ClaimsCacheInvalidatorService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task InvalidateAsync(string email, System.Guid? organisationId)
        {
            await _cache.RemoveAsync(ClaimsEnrichmentConstants.GetClientClaimsCacheKey(email, organisationId));
        }
    }
}
