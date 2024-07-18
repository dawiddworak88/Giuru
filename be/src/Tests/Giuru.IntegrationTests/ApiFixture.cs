using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Testcontainers.Elasticsearch;
using Testcontainers.MsSql;
using Testcontainers.Redis;

namespace Giuru.IntegrationTests
{
    public class ApiFixture : IAsyncLifetime
    {
        private INetwork _giuruNetwork;
        private RedisContainer _redisContainer;
        private MsSqlContainer _msSqlContainer;
        private ElasticsearchContainer _elasticsearchContainer;
        private IContainer _sellerWebContainer;

        public async Task InitializeAsync()
        {
            _giuruNetwork = new NetworkBuilder().Build();

            _redisContainer = new RedisBuilder()
                .WithName("redis")
                .WithNetwork(_giuruNetwork)
                .WithPortBinding(9113, 1433)
                .WithExposedPort(9113)
                .Build();

            _msSqlContainer = new MsSqlBuilder()
                .WithName("mssql")
                .WithNetwork(_giuruNetwork)
                .WithPassword("YourStrongPassword!")
                .WithPortBinding(9111, 1433)
                .WithExposedPort(9111)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
                .Build();

            await _msSqlContainer.StartAsync();

            _elasticsearchContainer = new ElasticsearchBuilder()
                .WithName("elasticsearch")
                .WithNetwork(_giuruNetwork)
                .WithPortBinding(9100, 9200)
                .WithExposedPort(9100)
                .Build();

            await _elasticsearchContainer.StartAsync();

            /*var sellerWebFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseSetting("ASPNETCORE_HTTP_PORTS", "8080");
                    builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Development");
                    builder.UseSetting("RedisUrl", "localhost:9113,abortConnect=false");
                    builder.UseSetting("EventBusConnection", _rabbitMqContainer.GetConnectionString());
                    builder.UseSetting("EventBusRetryCount", "5");
                    builder.UseSetting("EventBusRequestedHeartbeat", "60");
                });*/
        }

        public async Task DisposeAsync()
        {
            await _redisContainer.StopAsync();
            await _redisContainer.DisposeAsync();

            await _msSqlContainer.StopAsync();
            await _msSqlContainer.DisposeAsync();

            await _elasticsearchContainer.StopAsync();
            await _elasticsearchContainer.DisposeAsync();

            await _giuruNetwork.DisposeAsync();
        }
    }
}