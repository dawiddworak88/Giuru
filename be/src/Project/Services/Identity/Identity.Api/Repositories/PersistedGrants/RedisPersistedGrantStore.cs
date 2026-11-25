using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Identity.Api.Repositories.PersistedGrants
{
    // Keeps authorization codes and refresh tokens in Redis, so they survive restarts and deployments of Identity.Api
    public class RedisPersistedGrantStore : IPersistedGrantStore
    {
        private const string GrantKeyPrefix = "Identity-PersistedGrant-";
        private const string SubjectGrantsKeyPrefix = "Identity-PersistedGrants-Subject-";

        private readonly IDatabase database;

        public RedisPersistedGrantStore(IConnectionMultiplexer redis)
        {
            this.database = redis.GetDatabase();
        }

        public async Task StoreAsync(PersistedGrant grant)
        {
            if (grant.Expiration is null)
            {
                throw new NotSupportedException($"Persisted grants without expiration are not supported (grant type '{grant.Type}').");
            }

            var expiry = grant.Expiration.Value - DateTime.UtcNow;

            if (expiry <= TimeSpan.Zero)
            {
                await RemoveAsync(grant.Key);

                return;
            }

            await this.database.StringSetAsync(GetGrantKey(grant.Key), JsonSerializer.Serialize(grant), expiry);

            if (string.IsNullOrWhiteSpace(grant.SubjectId))
            {
                return;
            }

            var subjectGrantsKey = GetSubjectGrantsKey(grant.SubjectId);

            // Grants are indexed by subject for filtering, and the index lives as long as its longest-living grant
            await this.database.SetAddAsync(subjectGrantsKey, grant.Key);
            var subjectGrantsExpiry = await this.database.KeyTimeToLiveAsync(subjectGrantsKey);

            if (subjectGrantsExpiry is null || subjectGrantsExpiry < expiry)
            {
                await this.database.KeyExpireAsync(subjectGrantsKey, expiry);
            }
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            var value = await this.database.StringGetAsync(GetGrantKey(key));

            if (value.IsNullOrEmpty)
            {
                return null;
            }

            return JsonSerializer.Deserialize<PersistedGrant>(value.ToString());
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
        {
            filter.Validate();

            if (string.IsNullOrWhiteSpace(filter.SubjectId))
            {
                throw new NotSupportedException("Filtering persisted grants without a subject is not supported.");
            }

            var subjectGrantsKey = GetSubjectGrantsKey(filter.SubjectId);

            var keys = await this.database.SetMembersAsync(subjectGrantsKey);

            var grants = new List<PersistedGrant>();

            foreach (var key in keys)
            {
                var grant = await GetAsync(key.ToString());

                if (grant is null)
                {
                    // The grant has expired, so its key no longer belongs in the index
                    await this.database.SetRemoveAsync(subjectGrantsKey, key);

                    continue;
                }

                grants.Add(grant);
            }

            return grants.Where(grant =>
                (string.IsNullOrWhiteSpace(filter.ClientId) || grant.ClientId == filter.ClientId) &&
                (string.IsNullOrWhiteSpace(filter.SessionId) || grant.SessionId == filter.SessionId) &&
                (string.IsNullOrWhiteSpace(filter.Type) || grant.Type == filter.Type)).ToList();
        }

        public async Task RemoveAsync(string key)
        {
            var grant = await GetAsync(key);
            await this.database.KeyDeleteAsync(GetGrantKey(key));

            if (string.IsNullOrWhiteSpace(grant?.SubjectId) is false)
            {
                await this.database.SetRemoveAsync(GetSubjectGrantsKey(grant.SubjectId), key);
            }
        }

        public async Task RemoveAllAsync(PersistedGrantFilter filter)
        {
            var grants = await GetAllAsync(filter);

            foreach (var grant in grants)
            {
                await RemoveAsync(grant.Key);
            }
        }

        private static string GetGrantKey(string key) => $"{GrantKeyPrefix}{key}";

        private static string GetSubjectGrantsKey(string subjectId) => $"{SubjectGrantsKeyPrefix}{subjectId}";
    }
}
