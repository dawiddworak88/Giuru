using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Giuru.IntegrationTests.HttpClients;
using Giuru.IntegrationTests.Images;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private IContainer _mockAuthContainer;
        private IContainer _catalogApiContainer;
        private IContainer _catalogBackgroundTasksContainer;
        private IContainer _orderingApiContainer;
        private IContainer _basketApiContainer;
        public IContainer _clientApiContainer;

        public RestClient RestClient { get; private set; }

        public async Task InitializeAsync()
        {
            _giuruNetwork = new NetworkBuilder().Build();

            _redisContainer = new RedisBuilder()
                .WithName("redis")
                .WithNetwork(_giuruNetwork)
                .WithNetworkAliases("redis")
                .WithPortBinding(9113, 6379)
                .WithExposedPort(9113)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(6379))
                .Build();

            await _redisContainer.StartAsync();

            _msSqlContainer = new MsSqlBuilder()
                .WithName("mssql")
                .WithNetwork(_giuruNetwork)
                .WithPassword("YourStrongPassword!")
                .WithNetworkAliases("sqldata")
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
                .WithNetworkAliases("rabbitmq")
                .WithPortBinding(9000, 5672)
                .WithExposedPort(9000)
                .WithUsername("RMQ_USER")
                .WithPassword("YourStrongPassword!")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
                .Build();

            await _rabbitMqContainer.StartAsync();

            var mockAuthImage = new MockAuthImage();

            await mockAuthImage.InitializeAsync();

            _mockAuthContainer = new ContainerBuilder()
                .WithName("mock-auth")
                .WithImage(mockAuthImage)
                .WithNetwork(_giuruNetwork)
                .WithExposedPort(9105)
                .WithPortBinding(9105, 8080)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
                .WithEnvironment("EmailClaim", "seller@user.com")
                .WithEnvironment("RolesClaim", "Seller")
                .WithEnvironment("OrganisationClaim", "09affcc9-1665-45d6-919f-3d2026106ba1")
                .WithEnvironment("ExpiresInMinutes", "86400")
                .WithEnvironment("Issuer", "null")
                .WithEnvironment("Audience", "all")
                .WithEnvironment("IdentityUrl", "http://host.docker.internal:9105")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
                .Build();

            await _mockAuthContainer.StartAsync();

            /*var catalogApiImage = new CatalogApiImage();

            await catalogApiImage.InitializeAsync();

            _catalogApiContainer = new ContainerBuilder()
                .WithName("catalog-api")
                .WithImage(catalogApiImage)
                .WithNetwork(_giuruNetwork)
                .WithExposedPort(9101)
                .WithPortBinding(9101, 8080)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
                .WithEnvironment("RedisUrl", "redis")
                .WithEnvironment("ConnectionString", $"Server=sqldata;Database=CatalogDb;User Id=sa;Password=YourStrongPassword!;TrustServerCertificate=True")
                .WithEnvironment("ElasticsearchUrl", _elasticsearchContainer.GetConnectionString())
                .WithEnvironment("ElasticsearchIndex", "catalog")
                .WithEnvironment("EventBusConnection", "amqp://RMQ_USER:YourStrongPassword!@rabbitmq")
                .WithEnvironment("EventBusRetryCount", "5")
                .WithEnvironment("EventBusRequestedHeartbeat", "60")
                .WithEnvironment("IdentityUrl", "http://host.docker.internal:9105")
                .WithEnvironment("SupportedCultures", "de,en,pl")
                .WithEnvironment("DefaultCulture", "en")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
                .Build();

            await _catalogApiContainer.StartAsync();

            var catalogBackgroundTasksImage = new CatalogBackgroundTasksImage();

            await catalogBackgroundTasksImage.InitializeAsync();

            _catalogBackgroundTasksContainer = new ContainerBuilder()
                .WithName("catalog-background-tasks")
                .WithImage(catalogBackgroundTasksImage)
                .WithNetwork(_giuruNetwork)
                .WithExposedPort(9104)
                .WithPortBinding(9104, 8080)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
                .WithEnvironment("RedisUrl", "redis")
                .WithEnvironment("ConnectionString", $"Server=sqldata;Database=CatalogDb;User Id=sa;Password=YourStrongPassword!;TrustServerCertificate=True")
                .WithEnvironment("ElasticsearchUrl", _elasticsearchContainer.GetConnectionString())
                .WithEnvironment("ElasticsearchIndex", "catalog")
                .WithEnvironment("EventBusConnection", "amqp://RMQ_USER:YourStrongPassword!@rabbitmq")
                .WithEnvironment("EventBusRetryCount", "5")
                .WithEnvironment("EventBusRequestedHeartbeat", "60")
                .WithEnvironment("IdentityUrl", "http://host.docker.internal:9105")
                .WithEnvironment("SupportedCultures", "de,en,pl")
                .WithEnvironment("DefaultCulture", "en")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
                .Build();

            await _catalogBackgroundTasksContainer.StartAsync();

            var orderingApiImage = new OrderingApiImage();

            await orderingApiImage.InitializeAsync();

            _orderingApiContainer = new ContainerBuilder()
                .WithName("ordering-api")
                .WithImage(orderingApiImage)
                .WithNetwork(_giuruNetwork)
                .WithExposedPort(9102)
                .WithPortBinding(9102, 8080)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
                .WithEnvironment("RedisUrl", "redis")
                .WithEnvironment("ConnectionString", $"Server=sqldata;Database=OrderingDb;User Id=sa;Password=YourStrongPassword!;TrustServerCertificate=True")
                .WithEnvironment("EventBusConnection", "amqp://RMQ_USER:YourStrongPassword!@rabbitmq")
                .WithEnvironment("EventBusRetryCount", "5")
                .WithEnvironment("EventBusRequestedHeartbeat", "60")
                .WithEnvironment("IdentityUrl", "http://host.docker.internal:9105")
                .WithEnvironment("SupportedCultures", "de,en,pl")
                .WithEnvironment("DefaultCulture", "en")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
                .Build();

            await _orderingApiContainer.StartAsync();

            var basketApiImage = new BasketApiImage();

            await basketApiImage.InitializeAsync();

            _basketApiContainer = new ContainerBuilder()
                .WithName("basket-api")
                .WithImage(basketApiImage)
                .WithNetwork(_giuruNetwork)
                .WithExposedPort(9103)
                .WithPortBinding(9103, 8080)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
                .WithEnvironment("RedisUrl", "redis")
                .WithEnvironment("ConnectionString", "redis")
                .WithEnvironment("EventBusConnection", "amqp://RMQ_USER:YourStrongPassword!@rabbitmq")
                .WithEnvironment("EventBusRetryCount", "5")
                .WithEnvironment("EventBusRequestedHeartbeat", "60")
                .WithEnvironment("IdentityUrl", "http://host.docker.internal:9105")
                .WithEnvironment("SupportedCultures", "de,en,pl")
                .WithEnvironment("DefaultCulture", "en")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
                .Build();

            await _basketApiContainer.StartAsync();*/

            var clientApiImage = new ClientApiImage();

            await clientApiImage.InitializeAsync();

            _clientApiContainer = new ContainerBuilder()
                .WithName("client-api")
                .WithImage(clientApiImage)
                .WithNetwork(_giuruNetwork)
                .WithExposedPort(9106)
                .WithPortBinding(9106, 8080)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
                .WithEnvironment("RedisUrl", "redis")
                .WithEnvironment("ConnectionString", "Server=sqldata;Database=ClientDb;User Id=sa;Password=YourStrongPassword!;TrustServerCertificate=True")
                .WithEnvironment("EventBusConnection", "amqp://RMQ_USER:YourStrongPassword!@rabbitmq")
                .WithEnvironment("EventBusRetryCount", "5")
                .WithEnvironment("EventBusRequestedHeartbeat", "60")
                .WithEnvironment("IdentityUrl", "http://host.docker.internal:9105")
                .WithEnvironment("SupportedCultures", "de,en,pl")
                .WithEnvironment("DefaultCulture", "en")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
                .Build();

            await _clientApiContainer.StartAsync();

            /*            var sellerWebFactory = new WebApplicationFactory<Program>()
                            .WithWebHostBuilder(builder =>
                            {
                                builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Development");
                                builder.UseSetting("RedisUrl", "redis");
                            })
                            .CreateClient();*/

            var tokenClient = new TokenClient(new HttpClient());
            var token = await tokenClient.GetTokenAsync($"http://{_mockAuthContainer.Hostname}:{_mockAuthContainer.GetMappedPublicPort(8080)}/api/token");

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            RestClient = new RestClient(httpClient);
        }

        public async Task DisposeAsync()
        {
            await _redisContainer.StopAsync();
            await _redisContainer.DisposeAsync();

            await _msSqlContainer.StopAsync();
            await _msSqlContainer.DisposeAsync();

            await _elasticsearchContainer.StopAsync();
            await _elasticsearchContainer.DisposeAsync();

            await _mockAuthContainer.StopAsync();
            await _mockAuthContainer.DisposeAsync();

 /*           await _catalogApiContainer.StopAsync();
            await _catalogApiContainer.DisposeAsync();

            await _catalogBackgroundTasksContainer.StopAsync();
            await _catalogBackgroundTasksContainer.DisposeAsync();

            await _orderingApiContainer.StopAsync();
            await _orderingApiContainer.DisposeAsync();

            await _basketApiContainer.StopAsync();
            await _basketApiContainer.DisposeAsync();*/

            await _rabbitMqContainer.StopAsync();
            await _rabbitMqContainer.DisposeAsync();

            await _clientApiContainer.StopAsync();
            await _clientApiContainer.DisposeAsync();

            await _giuruNetwork.DisposeAsync();
        }
    }
}