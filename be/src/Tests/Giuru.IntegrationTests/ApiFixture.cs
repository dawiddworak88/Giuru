using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Giuru.IntegrationTests.Images;
using System.Threading.Tasks;
using Testcontainers.Elasticsearch;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace Giuru.IntegrationTests
{
    public class ApiFixture : IAsyncLifetime
    {
        private INetwork _giuruNetwork;
        private RedisContainer _redisContainer;
        private RabbitMqContainer _rabbitMqContainer;
        private MsSqlContainer _msSqlContainer;
        private ElasticsearchContainer _elasticsearchContainer;
        private IContainer _catalogContainer;
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

            await _redisContainer.StartAsync();

            _msSqlContainer = new MsSqlBuilder()
                .WithName("mssql")
                .WithNetwork(_giuruNetwork)
                //.WithNetworkAliases("sqldata")
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

            _rabbitMqContainer = new RabbitMqBuilder()
                .WithName("rabbitmq")
                .WithNetwork(_giuruNetwork)
                .Build();

            await _rabbitMqContainer.StartAsync();

            var catalogImage = new CatalogApiImage();

            await catalogImage.InitializeAsync();

            _catalogContainer = new ContainerBuilder()
                .WithName("catalog-api")
                .WithImage(catalogImage)
                .WithNetwork(_giuruNetwork)
                .WithExposedPort(9101)
                .WithPortBinding(9101, 8080)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
                .WithEnvironment("RedisUrl", $"host.docker.internal:9113,abortConnect=false")
                .WithEnvironment("ConnectionString", $"Server=host.docker.internal,9111;Database=CatalogDb;User Id=sa;Password=YourStrongPassword!;TrustServerCertificate=True")
                .WithEnvironment("ElasticsearchUrl", _elasticsearchContainer.GetConnectionString())
                .WithEnvironment("ElasticsearchIndex", "catalog")
                .WithEnvironment("EventBusConnection", _rabbitMqContainer.GetConnectionString())
                .WithEnvironment("EventBusRetryCount", "5")
                .WithEnvironment("EventBusRequestedHeartbeat", "60")
                .WithEnvironment("SupportedCultures", "de,en,pl")
                .WithEnvironment("DefaultCulture", "en")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
                .Build();

            await _catalogContainer.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _redisContainer.StopAsync();
            await _redisContainer.DisposeAsync();

            await _msSqlContainer.StopAsync();
            await _msSqlContainer.DisposeAsync();

            await _elasticsearchContainer.StopAsync();
            await _elasticsearchContainer.DisposeAsync();

            await _catalogContainer.StopAsync();
            await _catalogContainer.DisposeAsync();

            await _rabbitMqContainer.StopAsync();
            await _rabbitMqContainer.DisposeAsync();

            await _giuruNetwork.DisposeAsync();
        }
    }
}