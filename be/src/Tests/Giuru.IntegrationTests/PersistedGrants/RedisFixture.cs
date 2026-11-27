using StackExchange.Redis;
using System.Threading.Tasks;
using Testcontainers.Redis;

namespace Giuru.IntegrationTests.PersistedGrants
{
    public class RedisFixture : IAsyncLifetime
    {
        private readonly RedisContainer _redisContainer = new RedisBuilder("redis:latest").Build();

        public IConnectionMultiplexer Redis { get; private set; }

        public async Task InitializeAsync()
        {
            await _redisContainer.StartAsync();
            Redis = await ConnectionMultiplexer.ConnectAsync(_redisContainer.GetConnectionString());
        }

        public async Task DisposeAsync()
        {
            Redis.Dispose();

            await _redisContainer.DisposeAsync();
        }
    }
}
