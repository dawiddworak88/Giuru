using Foundation.Account.Definitions;
using Identity.Api.Repositories.PersistedGrants;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests.PersistedGrants
{
    public class RedisPersistedGrantStoreTests : IClassFixture<RedisFixture>
    {
        private const string RefreshTokenType = "refresh_token";
        private const string AuthorizationCodeType = "authorization_code";

        private readonly RedisFixture _redisFixture;
        private readonly RedisPersistedGrantStore _store;

        public RedisPersistedGrantStoreTests(RedisFixture redisFixture)
        {
            _redisFixture = redisFixture;
            _store = new RedisPersistedGrantStore(redisFixture.Redis);
        }

        [Fact]
        public async Task StoreAsync_Grant_CanBeReadBack()
        {
            var grant = CreateGrant(Guid.NewGuid().ToString(), RefreshTokenType, TimeSpan.FromDays(7));

            await _store.StoreAsync(grant);

            var storedGrant = await _store.GetAsync(grant.Key);

            Assert.Equal(grant.SubjectId, storedGrant.SubjectId);
            Assert.Equal(grant.ClientId, storedGrant.ClientId);
            Assert.Equal(grant.SessionId, storedGrant.SessionId);
            Assert.Equal(grant.Type, storedGrant.Type);
            Assert.Equal(grant.Data, storedGrant.Data);
            Assert.Equal(grant.CreationTime, storedGrant.CreationTime);
            Assert.Equal(grant.Expiration, storedGrant.Expiration);
        }

        [Fact]
        public async Task StoreAsync_AlreadyExpiredGrant_RemovesIt()
        {
            var grant = CreateGrant(Guid.NewGuid().ToString(), RefreshTokenType, TimeSpan.FromDays(7));

            await _store.StoreAsync(grant);

            grant.Expiration = DateTime.UtcNow.AddSeconds(-1);

            await _store.StoreAsync(grant);

            Assert.Null(await _store.GetAsync(grant.Key));
        }

        [Fact]
        public async Task StoreAsync_ShortLivedGrant_KeepsSubjectIndexForTheLongestGrantLifetime()
        {
            var subjectId = Guid.NewGuid().ToString();

            await _store.StoreAsync(CreateGrant(subjectId, AuthorizationCodeType, TimeSpan.FromMinutes(5)));

            var indexExpiry = await _redisFixture.Redis.GetDatabase().KeyTimeToLiveAsync(GetSubjectGrantsKey(subjectId));
            var longestGrantLifetime = TimeSpan.FromSeconds(AccountConstants.TokenLifetimes.AbsoluteRefreshTokenLifetimeInSeconds);

            Assert.True(indexExpiry > longestGrantLifetime - TimeSpan.FromMinutes(1));
        }

        [Fact]
        public async Task StoreAsync_GrantWithoutSubject_StoresItWithoutSubjectIndex()
        {
            var grant = CreateGrant(null, AuthorizationCodeType, TimeSpan.FromMinutes(5));

            await _store.StoreAsync(grant);

            Assert.NotNull(await _store.GetAsync(grant.Key));
            Assert.False(await _redisFixture.Redis.GetDatabase().KeyExistsAsync(GetSubjectGrantsKey(null)));
        }

        [Fact]
        public async Task GetAllAsync_FilterByType_ReturnsOnlyThatType()
        {
            var subjectId = Guid.NewGuid().ToString();
            var refreshToken = CreateGrant(subjectId, RefreshTokenType, TimeSpan.FromDays(7));

            await _store.StoreAsync(refreshToken);
            await _store.StoreAsync(CreateGrant(subjectId, AuthorizationCodeType, TimeSpan.FromMinutes(5)));

            var grants = await _store.GetAllAsync(new PersistedGrantFilter { SubjectId = subjectId, Type = RefreshTokenType });

            Assert.Equal(new[] { refreshToken.Key }, grants.Select(grant => grant.Key));
        }

        [Fact]
        public async Task GetAllAsync_GrantExpiredInRedis_SkipsItAndRemovesItFromSubjectIndex()
        {
            var subjectId = Guid.NewGuid().ToString();
            var refreshToken = CreateGrant(subjectId, RefreshTokenType, TimeSpan.FromDays(7));
            var authorizationCode = CreateGrant(subjectId, AuthorizationCodeType, TimeSpan.FromMinutes(5));
            var database = _redisFixture.Redis.GetDatabase();

            await _store.StoreAsync(refreshToken);
            await _store.StoreAsync(authorizationCode);
            await database.KeyDeleteAsync($"Identity-PersistedGrant-{authorizationCode.Key}");

            var grants = await _store.GetAllAsync(new PersistedGrantFilter { SubjectId = subjectId });

            Assert.Equal(new[] { refreshToken.Key }, grants.Select(grant => grant.Key));
            Assert.False(await database.SetContainsAsync(GetSubjectGrantsKey(subjectId), authorizationCode.Key));
        }

        [Fact]
        public async Task RemoveAllAsync_Subject_RemovesOnlyThatSubjectsGrants()
        {
            var subjectId = Guid.NewGuid().ToString();
            var refreshToken = CreateGrant(subjectId, RefreshTokenType, TimeSpan.FromDays(7));
            var authorizationCode = CreateGrant(subjectId, AuthorizationCodeType, TimeSpan.FromMinutes(5));
            var otherSubjectRefreshToken = CreateGrant(Guid.NewGuid().ToString(), RefreshTokenType, TimeSpan.FromDays(7));

            await _store.StoreAsync(refreshToken);
            await _store.StoreAsync(authorizationCode);
            await _store.StoreAsync(otherSubjectRefreshToken);

            await _store.RemoveAllAsync(new PersistedGrantFilter { SubjectId = subjectId });

            Assert.Null(await _store.GetAsync(refreshToken.Key));
            Assert.Null(await _store.GetAsync(authorizationCode.Key));
            Assert.NotNull(await _store.GetAsync(otherSubjectRefreshToken.Key));
        }

        private static PersistedGrant CreateGrant(string subjectId, string type, TimeSpan lifetime)
        {
            var now = DateTime.UtcNow;

            return new PersistedGrant
            {
                Key = Guid.NewGuid().ToString(),
                Type = type,
                SubjectId = subjectId,
                ClientId = "buyer-web",
                SessionId = "session-id",
                CreationTime = now,
                Expiration = now.Add(lifetime),
                Data = "{}"
            };
        }

        private static string GetSubjectGrantsKey(string subjectId) => $"Identity-PersistedGrants-Subject-{subjectId}";
    }
}
