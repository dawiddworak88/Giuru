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

        public Task InvalidateAsync(System.Guid? organisationId)
        {
            return ClientClaimsCacheVersion.RenewAsync(_cache, organisationId);
        }
    }
}
