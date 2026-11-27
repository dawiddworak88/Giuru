using Foundation.Account.Definitions;
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
    public class RedisPersistedGrantStore : IPersistedGrantStore
    {
        private const string GrantKeyPrefix = "Identity-PersistedGrant-";
        private const string SubjectGrantsKeyPrefix = "Identity-PersistedGrants-Subject-";

        // Outlives every grant, so concurrent stores can't shorten it
        private static readonly TimeSpan SubjectGrantsExpiry = TimeSpan.FromSeconds(AccountConstants.TokenLifetimes.AbsoluteRefreshTokenLifetimeInSeconds);

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

            // One MULTI/EXEC, so a dropped connection can't leave a grant unindexed
            var transaction = this.database.CreateTransaction();
            var commands = new List<Task>
            {
                transaction.StringSetAsync(GetGrantKey(grant.Key), JsonSerializer.Serialize(grant), expiry)
            };

            if (string.IsNullOrWhiteSpace(grant.SubjectId) is false)
            {
                var subjectGrantsKey = GetSubjectGrantsKey(grant.SubjectId);

                commands.Add(transaction.SetAddAsync(subjectGrantsKey, grant.Key));
                commands.Add(transaction.KeyExpireAsync(subjectGrantsKey, expiry > SubjectGrantsExpiry ? expiry : SubjectGrantsExpiry));
            }

            await transaction.ExecuteAsync();
            await Task.WhenAll(commands);
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
