using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Giuru.IntegrationTests.Definitions;
using Giuru.IntegrationTests.HttpClients;
using Giuru.IntegrationTests.Images;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
        private IContainer _elasticsearchContainer;
        private IContainer _mockAuthContainer;
        private IContainer _clientApiContainer;
        private IContainer _catalogApiContainer;
        private IContainer _catalogBackgroundTasksContainer;
        private IContainer _orderingApiContainer;
        private IContainer _basketApiContainer;
        private IContainer _inventoryApiContainer;

        public RestClient SellerWebClient { get; private set; }
        public RestClient BuyerWebClient { get; private set; }

        public async Task InitializeAsync()
        {
            _giuruNetwork = new NetworkBuilder().Build();

            _redisContainer = new RedisBuilder()
                .WithName("redis")
                .WithNetwork(_giuruNetwork)
                .WithNetworkAliases("redis")
                .WithPortBinding(9113, 6379)
                .WithExposedPort(6379)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(6379))
                .Build();

            await _redisContainer.StartAsync();

            _msSqlContainer = new MsSqlBuilder()
                .WithName("mssql")
                .WithImage("mcr.microsoft.com/mssql/server:2022-CU15-GDR1-ubuntu-22.04")
                .WithNetwork(_giuruNetwork)
                .WithPassword("YourStrongPassword!")
                .WithNetworkAliases("sqldata")
                .WithPortBinding(9111, 1433)
                .WithExposedPort(1433)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
                .Build();

            await _msSqlContainer.StartAsync();

            _elasticsearchContainer = new ContainerBuilder()
                .WithName("elasticsearch")
                .WithNetwork(_giuruNetwork)
                .WithNetworkAliases("elasticsearch")
                .WithImage("docker.elastic.co/elasticsearch/elasticsearch:7.9.1")
                .WithEnvironment("discovery.type", "single-node")
                .WithEnvironment("xpack.security.enabled", "false")
                .WithEnvironment("xpack.security.http.ssl.enabled", "false")
                .WithExposedPort(9200)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(9200))
                .Build();

            await _elasticsearchContainer.StartAsync();

            _rabbitMqContainer = new RabbitMqBuilder()
                .WithName("rabbitmq")
                .WithNetwork(_giuruNetwork)
                .WithNetworkAliases("rabbitmq")
                .WithPortBinding(9000, 5672)
                .WithExposedPort(5672)
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
                .WithExposedPort(8080)
                .WithPortBinding(9105, 8080)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
                .WithEnvironment("EmailClaim", "seller@user.com")
                .WithEnvironment("RolesClaim", "Seller")
                .WithEnvironment("OrganisationId", "09affcc9-1665-45d6-919f-3d2026106ba1")
                .WithEnvironment("ExpiresInMinutes", "86400")
                .WithEnvironment("Issuer", "null")
                .WithEnvironment("Audience", "all")
                .WithBindMount(Path.Combine(CommonDirectoryPath.GetProjectDirectory().DirectoryPath, "../Giuru.MockAuth/tempkey.jwk"), "/app/tempkey.jwk")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
                .Build();

            await _mockAuthContainer.StartAsync();

            var clientApiImage = new ClientApiImage();

            await clientApiImage.InitializeAsync();

            _clientApiContainer = new ContainerBuilder()
                .WithName("client-api")
                .WithImage(clientApiImage)
                .WithNetwork(_giuruNetwork)
                .WithExposedPort(8080)
                .WithPortBinding(9106, 8080)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
                .WithEnvironment("RedisUrl", "redis")
                .WithEnvironment("ConnectionString", "Server=sqldata;Database=ClientDb;User Id=sa;Password=YourStrongPassword!;TrustServerCertificate=True")
                .WithEnvironment("EventBusConnection", "amqp://RMQ_USER:YourStrongPassword!@rabbitmq")
                .WithEnvironment("EventBusRetryCount", "5")
                .WithEnvironment("EventBusRequestedHeartbeat", "60")
                .WithEnvironment("IdentityUrl", "http://mock-auth:8080")
                .WithEnvironment("SupportedCultures", "de,en,pl")
                .WithEnvironment("DefaultCulture", "en")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
                .Build();

            await _clientApiContainer.StartAsync();

            var catalogApiImage = new CatalogApiImage();

            await catalogApiImage.InitializeAsync();

            _catalogApiContainer = new ContainerBuilder()
                .WithName("catalog-api")
                .WithImage(catalogApiImage)
                .WithNetwork(_giuruNetwork)
                .WithExposedPort(8080)
                .WithPortBinding(9101, 8080)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
                .WithEnvironment("RedisUrl", "redis")
                .WithEnvironment("ConnectionString", $"Server=sqldata;Database=CatalogDb;User Id=sa;Password=YourStrongPassword!;TrustServerCertificate=True")
                .WithEnvironment("ElasticsearchUrl", "http://elasticsearch:9200")
                .WithEnvironment("ElasticsearchIndex", "catalog")
                .WithEnvironment("EventBusConnection", "amqp://RMQ_USER:YourStrongPassword!@rabbitmq")
                .WithEnvironment("EventBusRetryCount", "5")
                .WithEnvironment("EventBusRequestedHeartbeat", "60")
                .WithEnvironment("IdentityUrl", "http://mock-auth:8080")
                .WithEnvironment("Brands", "4a8f8442-43b0-4223-83bb-978d5e81acc7&ELTAP&09affcc9-1665-45d6-919f-3d2026106ba1")
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
                .WithExposedPort(8080)
                .WithPortBinding(9104, 8080)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
                .WithEnvironment("RedisUrl", "redis")
                .WithEnvironment("ConnectionString", $"Server=sqldata;Database=CatalogDb;User Id=sa;Password=YourStrongPassword!;TrustServerCertificate=True")
                .WithEnvironment("ElasticsearchUrl", "http://elasticsearch:9200")
                .WithEnvironment("ElasticsearchIndex", "catalog")
                .WithEnvironment("EventBusConnection", "amqp://RMQ_USER:YourStrongPassword!@rabbitmq")
                .WithEnvironment("EventBusRetryCount", "5")
                .WithEnvironment("EventBusRequestedHeartbeat", "60")
                .WithEnvironment("IdentityUrl", "http://mock-auth:8080")
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
                .WithExposedPort(8080)
                .WithPortBinding(9102, 8080)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
                .WithEnvironment("RedisUrl", "redis")
                .WithEnvironment("ConnectionString", $"Server=sqldata;Database=OrderingDb;User Id=sa;Password=YourStrongPassword!;TrustServerCertificate=True")
                .WithEnvironment("EventBusConnection", "amqp://RMQ_USER:YourStrongPassword!@rabbitmq")
                .WithEnvironment("EventBusRetryCount", "5")
                .WithEnvironment("EventBusRequestedHeartbeat", "60")
                .WithEnvironment("IdentityUrl", "http://mock-auth:8080")
                .WithEnvironment("SendGridApiKey", "SIMPLE_SENDGRID_API_KEY")
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
                .WithExposedPort(8080)
                .WithPortBinding(9103, 8080)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
                .WithEnvironment("RedisUrl", "redis")
                .WithEnvironment("ConnectionString", "redis")
                .WithEnvironment("EventBusConnection", "amqp://RMQ_USER:YourStrongPassword!@rabbitmq")
                .WithEnvironment("EventBusRetryCount", "5")
                .WithEnvironment("EventBusRequestedHeartbeat", "60")
                .WithEnvironment("IdentityUrl", "http://mock-auth:8080")
                .WithEnvironment("SupportedCultures", "de,en,pl")
                .WithEnvironment("DefaultCulture", "en")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
                .Build();

            await _basketApiContainer.StartAsync();

            var inventoryApiImage = new InventoryApiImage();

            await inventoryApiImage.InitializeAsync();

            _inventoryApiContainer = new ContainerBuilder()
                .WithName("inventory-api")
                .WithImage(inventoryApiImage)
                .WithNetwork(_giuruNetwork)
                .WithExposedPort(8080)
                .WithPortBinding(9107, 8080)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
                .WithEnvironment("RedisUrl", "redis")
                .WithEnvironment("ConnectionString", $"Server=sqldata;Database=InventoryDb;User Id=sa;Password=YourStrongPassword!;TrustServerCertificate=True")
                .WithEnvironment("EventBusConnection", "amqp://RMQ_USER:YourStrongPassword!@rabbitmq")
                .WithEnvironment("EventBusRetryCount", "5")
                .WithEnvironment("EventBusRequestedHeartbeat", "60")
                .WithEnvironment("IdentityUrl", "http://mock-auth:8080")
                .WithEnvironment("SupportedCultures", "de,en,pl")
                .WithEnvironment("DefaultCulture", "en")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
                .Build();

            await _inventoryApiContainer.StartAsync();

            var tokenClient = new TokenClient(new HttpClient());
            var token = await tokenClient.GetTokenAsync($"http://{_mockAuthContainer.Hostname}:{_mockAuthContainer.GetMappedPublicPort(8080)}/api/token");

            var sellerWebFactpry = new WebApplicationFactory<SellerWebProgram>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseSetting("ASPNETCORE_HTTP_PORTS", "8080");
                    builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Development");
                    builder.UseSetting("RedisUrl", "redis,abortConnect=false");
                    builder.UseSetting("ClientId", "663bba90-0036-4a58-8516-39faa8baba87");
                    builder.UseSetting("ClientSecret", "c61fcb32-cf9b-4cdd-84dc-4a1b173c36e9");
                    builder.UseSetting("ClientUrl", $"http://{_clientApiContainer.Hostname}:{_clientApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("CatalogUrl", $"http://{_catalogApiContainer.Hostname}:{_catalogApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("InventoryUrl", $"http://{_inventoryApiContainer.Hostname}:{_inventoryApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("IdentityUrl", $"http://{_mockAuthContainer.Hostname}:{_mockAuthContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("IntegrationTestsEnabled", "true");
                    builder.UseSetting("SupportedCultures", "de,en,pl");
                    builder.UseSetting("DefaultCulture", "en");
                })
                .CreateClient();

            sellerWebFactpry.DefaultRequestHeaders.Accept.Clear();
            sellerWebFactpry.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            sellerWebFactpry.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            SellerWebClient = new RestClient(sellerWebFactpry);

            var buyerWebFactpry = new WebApplicationFactory<BuyerWebProgram>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseSetting("ASPNETCORE_HTTP_PORTS", "8080");
                    builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Development");
                    builder.UseSetting("RedisUrl", "redis,abortConnect=false");
                    builder.UseSetting("ClientId", "663bba90-0036-4a58-8516-39faa8baba87");
                    builder.UseSetting("ClientSecret", "c61fcb32-cf9b-4cdd-84dc-4a1b173c36e9");
                    builder.UseSetting("OrderUrl", $"http://{_orderingApiContainer.Hostname}:{_orderingApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("ClientUrl", $"http://{_clientApiContainer.Hostname}:{_clientApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("CatalogUrl", $"http://{_catalogApiContainer.Hostname}:{_catalogApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("InventoryUrl", $"http://{_inventoryApiContainer.Hostname}:{_inventoryApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("BasketUrl", $"http://{_basketApiContainer.Hostname}:{_basketApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("IdentityUrl", $"http://{_mockAuthContainer.Hostname}:{_mockAuthContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("IntegrationTestsEnabled", "true");
                    builder.UseSetting("SupportedCultures", "de,en,pl");
                    builder.UseSetting("DefaultCulture", "en");
                })
                .CreateClient();

            buyerWebFactpry.DefaultRequestHeaders.Accept.Clear();
            buyerWebFactpry.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            buyerWebFactpry.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            BuyerWebClient = new RestClient(buyerWebFactpry);
/*
            await WaitUntilAuthReadyAsync(sellerWebFactpry, "SellerWeb", "/__test/auth-ready");
            await WaitUntilAuthReadyAsync(buyerWebFactpry, "BuyerWeb", "/__test/auth-ready");*/
        }

        public async Task DisposeAsync()
        {
            await _giuruNetwork.DisposeAsync();

            await _redisContainer.StopAsync();
            await _redisContainer.DisposeAsync();

            await _msSqlContainer.StopAsync();
            await _msSqlContainer.DisposeAsync();

            await _elasticsearchContainer.StopAsync();
            await _elasticsearchContainer.DisposeAsync();

            await _rabbitMqContainer.StopAsync();
            await _rabbitMqContainer.DisposeAsync();

            await _mockAuthContainer.StopAsync();
            await _mockAuthContainer.DisposeAsync();

            await _clientApiContainer.StopAsync();
            await _clientApiContainer.DisposeAsync();

            await _catalogApiContainer.StopAsync();
            await _catalogApiContainer.DisposeAsync();

            await _catalogBackgroundTasksContainer.StopAsync();
            await _catalogBackgroundTasksContainer.DisposeAsync();

            await _orderingApiContainer.StopAsync();
            await _orderingApiContainer.DisposeAsync();

            await _basketApiContainer.StopAsync();
            await _basketApiContainer.DisposeAsync();

            await _inventoryApiContainer.StopAsync();
            await _inventoryApiContainer.DisposeAsync();
        }

       /* private static async Task WaitUntilAuthReadyAsync(HttpClient client, string name, string endpointUrl)
        {
            var deadline = DateTime.UtcNow.AddSeconds(30);
            HttpResponseMessage? last = null;

            while (DateTime.UtcNow < deadline)
            {
                try
                {
                    last = await client.GetAsync(endpointUrl);

                    if (last.IsSuccessStatusCode)
                        return;
                }
                catch (Exception ex)
                {
                    throw new Exception($"{name} auth readiness check failed: {ex.Message}", ex);
                    // serwis jeszcze siê rozkrêca
                }

                await Task.Delay(300);
            }

            var status = last is null ? "(no response)" : $"{(int)last.StatusCode} {last.ReasonPhrase}";
            throw new TimeoutException($"{name} auth not ready after 30s. Last status: {status}");
        }*/
    }
}