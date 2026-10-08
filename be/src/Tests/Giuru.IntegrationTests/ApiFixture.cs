using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
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
        private const string UnreachableGrulaUrl = "http://127.0.0.1:1";
        private const string TestGrulaAccessToken = "integration-test-token";
        private const string TestGrulaEnvironmentId = "00000000-0000-0000-0000-000000000001";

        // Pricing a catalogued product translates its colour, which reads the items of this attribute. None exist here, so
        // the lookup answers with an empty list instead of failing on a missing attribute id.
        private const string TestProductColorAttributeId = "a04b3368-fa25-4b4a-e4eb-08d907680a85";

        private INetwork _giuruNetwork;
        private RedisContainer _redisContainer;
        private RabbitMqContainer _rabbitMqContainer;
        private MsSqlContainer _msSqlContainer;
        private IContainer _elasticsearchContainer;
        private IContainer _mockAuthContainer;
        private IContainer _clientApiContainer;
        private IContainer _globalApiContainer;
        private IContainer _catalogApiContainer;
        private IContainer _catalogBackgroundTasksContainer;
        private IContainer _orderingApiContainer;
        private IContainer _basketApiContainer;
        private IContainer _inventoryApiContainer;

        private string _mockAuthTokenEndpoint;
        private string _clientApiUrl;
        private string _globalApiUrl;
        private string _basketApiUrl;
        private string _webRedisUrl;
        private WebApplicationFactory<SellerWebProgram> _sellerWebFactory;
        private WebApplicationFactory<SellerWebProgram> _enforcedSellerWebFactory;
        private WebApplicationFactory<BuyerWebProgram> _enforcedBuyerWebFactory;
        private WebApplicationFactory<SellerWebProgram> _clientApiUnavailableSellerWebFactory;

        public RestClient SellerWebClient { get; private set; }
        public RestClient BuyerWebClient { get; private set; }
        public RestClient BasketApiClient { get; private set; }

        /// <summary>Client.Api called with the default seller token. Discount codes are seeded here, because Seller.Web cannot verify them against the unreachable Grula.</summary>
        public RestClient ClientApiClient { get; private set; }

        /// <summary>
        /// Seller.Web and Buyer.Web run twice over the same services: the default pair keeps discount code enforcement off
        /// (the existing behaviour), the enforced pair has DiscountCodeEnforcementEnabled on.
        /// </summary>
        public RestClient EnforcedSellerWebClient { get; private set; }
        public RestClient EnforcedBuyerWebClient { get; private set; }

        public async Task InitializeAsync()
        {
            _giuruNetwork = new NetworkBuilder().Build();

            _redisContainer = new RedisBuilder("redis:latest")
                .WithName("redis")
                .WithNetwork(_giuruNetwork)
                .WithNetworkAliases("redis")
                .WithPortBinding(9113, 6379)
                .WithExposedPort(6379)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilInternalTcpPortIsAvailable(6379))
                .Build();

            await _redisContainer.StartAsync();

            _msSqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-CU15-GDR1-ubuntu-22.04")
                .WithName("mssql")
                .WithNetwork(_giuruNetwork)
                .WithPassword("YourStrongPassword!")
                .WithNetworkAliases("sqldata")
                .WithPortBinding(9111, 1433)
                .WithExposedPort(1433)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilInternalTcpPortIsAvailable(1433))
                .Build();

            await _msSqlContainer.StartAsync();

            _elasticsearchContainer = new ContainerBuilder("docker.elastic.co/elasticsearch/elasticsearch:7.9.1")
                .WithName("elasticsearch")
                .WithNetwork(_giuruNetwork)
                .WithNetworkAliases("elasticsearch")
                .WithEnvironment("discovery.type", "single-node")
                .WithEnvironment("xpack.security.enabled", "false")
                .WithEnvironment("xpack.security.http.ssl.enabled", "false")
                .WithEnvironment("ES_JAVA_OPTS", "-Xms512m -Xmx512m")
                .WithExposedPort(9200)
                .WithPortBinding(9200, 9200)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r
                    .ForPort(9200)
                    .ForPath("/_cluster/health")
                    .ForStatusCode(System.Net.HttpStatusCode.OK)))
                .Build();

            await _elasticsearchContainer.StartAsync();

            _rabbitMqContainer = new RabbitMqBuilder("rabbitmq:latest")
                .WithName("rabbitmq")
                .WithNetwork(_giuruNetwork)
                .WithNetworkAliases("rabbitmq")
                .WithPortBinding(9000, 5672)
                .WithExposedPort(5672)
                .WithUsername("RMQ_USER")
                .WithPassword("YourStrongPassword!")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilInternalTcpPortIsAvailable(5672))
                .Build();

            await _rabbitMqContainer.StartAsync();

            var mockAuthImage = new MockAuthImage();

            await mockAuthImage.InitializeAsync();

            _mockAuthContainer = new ContainerBuilder(mockAuthImage)
                .WithName("mock-auth")
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
                .WithWaitStrategy(Wait.ForUnixContainer().UntilInternalTcpPortIsAvailable(8080))
                .Build();

            await _mockAuthContainer.StartAsync();

            var clientApiImage = new ClientApiImage();

            await clientApiImage.InitializeAsync();

            _clientApiContainer = new ContainerBuilder(clientApiImage)
                .WithName("client-api")
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
                .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r
                    .ForPort(8080)
                    .ForPath("/liveness")
                    .ForStatusCode(System.Net.HttpStatusCode.OK)))
                .Build();

            await _clientApiContainer.StartAsync();

            // Pricing for a client reads its country and currency, and so does the buyer's claims enrichment: both web
            // apps need the Global API as soon as a request is made for a real client.
            var globalApiImage = new GlobalApiImage();

            await globalApiImage.InitializeAsync();

            _globalApiContainer = new ContainerBuilder(globalApiImage)
                .WithName("global-api")
                .WithNetwork(_giuruNetwork)
                .WithExposedPort(8080)
                .WithPortBinding(9108, 8080)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
                .WithEnvironment("RedisUrl", "redis")
                .WithEnvironment("ConnectionString", "Server=sqldata;Database=GlobalDb;User Id=sa;Password=YourStrongPassword!;TrustServerCertificate=True")
                .WithEnvironment("IdentityUrl", "http://mock-auth:8080")
                .WithEnvironment("SupportedCultures", "de,en,pl")
                .WithEnvironment("DefaultCulture", "en")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r
                    .ForPort(8080)
                    .ForPath("/liveness")
                    .ForStatusCode(System.Net.HttpStatusCode.OK)))
                .Build();

            await _globalApiContainer.StartAsync();

            var catalogApiImage = new CatalogApiImage();

            await catalogApiImage.InitializeAsync();

            _catalogApiContainer = new ContainerBuilder(catalogApiImage)
                .WithName("catalog-api")
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
                .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r
                    .ForPort(8080)
                    .ForPath("/liveness")
                    .ForStatusCode(System.Net.HttpStatusCode.OK)))
                .Build();

            await _catalogApiContainer.StartAsync();

            var catalogBackgroundTasksImage = new CatalogBackgroundTasksImage();

            await catalogBackgroundTasksImage.InitializeAsync();

            _catalogBackgroundTasksContainer = new ContainerBuilder(catalogBackgroundTasksImage)
                .WithName("catalog-background-tasks")
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
                .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r
                    .ForPort(8080)
                    .ForPath("/liveness")
                    .ForStatusCode(System.Net.HttpStatusCode.OK)))
                .Build();

            await _catalogBackgroundTasksContainer.StartAsync();

            var orderingApiImage = new OrderingApiImage();

            await orderingApiImage.InitializeAsync();

            _orderingApiContainer = new ContainerBuilder(orderingApiImage)
                .WithName("ordering-api")
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
                .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r
                    .ForPort(8080)
                    .ForPath("/liveness")
                    .ForStatusCode(System.Net.HttpStatusCode.OK)))
                .Build();

            await _orderingApiContainer.StartAsync();

            var basketApiImage = new BasketApiImage();

            await basketApiImage.InitializeAsync();

            _basketApiContainer = new ContainerBuilder(basketApiImage)
                .WithName("basket-api")
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
                .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r
                    .ForPort(8080)
                    .ForPath("/liveness")
                    .ForStatusCode(System.Net.HttpStatusCode.OK)))
                .Build();

            await _basketApiContainer.StartAsync();

            var inventoryApiImage = new InventoryApiImage();

            await inventoryApiImage.InitializeAsync();

            _inventoryApiContainer = new ContainerBuilder(inventoryApiImage)
                .WithName("inventory-api")
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
                .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r
                    .ForPort(8080)
                    .ForPath("/liveness")
                    .ForStatusCode(System.Net.HttpStatusCode.OK)))
                .Build();

            await _inventoryApiContainer.StartAsync();

            _mockAuthTokenEndpoint = $"http://{_mockAuthContainer.Hostname}:{_mockAuthContainer.GetMappedPublicPort(8080)}/api/token";
            _clientApiUrl = $"http://{_clientApiContainer.Hostname}:{_clientApiContainer.GetMappedPublicPort(8080)}";
            _globalApiUrl = $"http://{_globalApiContainer.Hostname}:{_globalApiContainer.GetMappedPublicPort(8080)}";
            _basketApiUrl = $"http://{_basketApiContainer.Hostname}:{_basketApiContainer.GetMappedPublicPort(8080)}";

            // The web apps run in this process, outside the container network, so they reach Redis through its mapped
            // port: the "redis" alias only resolves between the containers.
            _webRedisUrl = $"{_redisContainer.Hostname}:{_redisContainer.GetMappedPublicPort(6379)},abortConnect=false";

            var tokenClient = new TokenClient(new HttpClient());
            var token = await tokenClient.GetTokenAsync(_mockAuthTokenEndpoint);

            BasketApiClient = CreateBasketApiClient(token);

            _sellerWebFactory = CreateSellerWebFactory(enforceDiscountCodes: false);
            var buyerWebFactory = CreateBuyerWebFactory(enforceDiscountCodes: false);

            _enforcedSellerWebFactory = CreateSellerWebFactory(enforceDiscountCodes: true);
            _enforcedBuyerWebFactory = CreateBuyerWebFactory(enforceDiscountCodes: true);

            // Enforcement is on but Client.Api cannot be reached, so no code can be verified.
            _clientApiUnavailableSellerWebFactory = CreateSellerWebFactory(enforceDiscountCodes: true, clientUrl: UnreachableGrulaUrl);

            SellerWebClient = CreateRestClient(_sellerWebFactory.CreateClient(), token);
            BuyerWebClient = CreateRestClient(buyerWebFactory.CreateClient(), token);
            EnforcedSellerWebClient = CreateRestClient(_enforcedSellerWebFactory.CreateClient(), token);
            EnforcedBuyerWebClient = CreateRestClient(_enforcedBuyerWebFactory.CreateClient(), token);
            ClientApiClient = CreateClientApiClient(token);
        }

        /// <summary>Issues a mock token for another identity. A null role keeps the configured seller role, "none" issues no role at all.</summary>
        public Task<string> GetTokenAsync(string email, string role, Guid organisationId)
        {
            var query = $"?email={Uri.EscapeDataString(email)}&role={Uri.EscapeDataString(role ?? string.Empty)}&organisationId={organisationId}";

            return new TokenClient(new HttpClient()).GetTokenAsync(_mockAuthTokenEndpoint + query);
        }

        public RestClient CreateClientApiClient(string token) => CreateRestClient(CreateHttpClient(_clientApiUrl), token);

        public RestClient CreateBasketApiClient(string token) => CreateRestClient(CreateHttpClient(_basketApiUrl), token);

        /// <summary>A new client of the default Seller.Web (enforcement off) for another identity.</summary>
        public RestClient CreateSellerWebClient(string token) => CreateRestClient(_sellerWebFactory.CreateClient(), token);

        /// <summary>A new client of the enforced Seller.Web for another identity. Every call gets its own cookie container.</summary>
        public RestClient CreateEnforcedSellerWebClient(string token) => CreateRestClient(_enforcedSellerWebFactory.CreateClient(), token);

        /// <summary>A new client of the enforced Buyer.Web for another identity. Every call gets its own cookie container, so its own basket.</summary>
        public RestClient CreateEnforcedBuyerWebClient(string token) => CreateRestClient(_enforcedBuyerWebFactory.CreateClient(), token);

        /// <summary>A client of an enforced Seller.Web whose Client.Api is unreachable.</summary>
        public RestClient CreateClientApiUnavailableSellerWebClient(string token) => CreateRestClient(_clientApiUnavailableSellerWebFactory.CreateClient(), token);

        private static HttpClient CreateHttpClient(string baseAddress) => new() { BaseAddress = new Uri(baseAddress) };

        private static RestClient CreateRestClient(HttpClient httpClient, string token)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return new RestClient(httpClient);
        }

        private WebApplicationFactory<SellerWebProgram> CreateSellerWebFactory(bool enforceDiscountCodes, string clientUrl = null)
        {
            return new WebApplicationFactory<SellerWebProgram>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseSetting("ASPNETCORE_HTTP_PORTS", "8080");
                    builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Development");
                    builder.UseSetting("RedisUrl", _webRedisUrl);
                    builder.UseSetting("ClientId", "663bba90-0036-4a58-8516-39faa8baba87");
                    builder.UseSetting("ClientSecret", "c61fcb32-cf9b-4cdd-84dc-4a1b173c36e9");
                    builder.UseSetting("ClientUrl", clientUrl ?? _clientApiUrl);
                    builder.UseSetting("GlobalUrl", _globalApiUrl);
                    builder.UseSetting("ProductColorAttributeId", TestProductColorAttributeId);
                    builder.UseSetting("CatalogUrl", $"http://{_catalogApiContainer.Hostname}:{_catalogApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("InventoryUrl", $"http://{_inventoryApiContainer.Hostname}:{_inventoryApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("BasketUrl", _basketApiUrl);
                    builder.UseSetting("IdentityUrl", $"http://{_mockAuthContainer.Hostname}:{_mockAuthContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("GrulaUrl", UnreachableGrulaUrl);
                    builder.UseSetting("GrulaAccessToken", TestGrulaAccessToken);
                    builder.UseSetting("GrulaEnvironmentId", TestGrulaEnvironmentId);
                    builder.UseSetting("DiscountCodeEnforcementEnabled", enforceDiscountCodes.ToString());
                    builder.UseSetting("IntegrationTestsEnabled", "true");
                    builder.UseSetting("SupportedCultures", "de,en,pl");
                    builder.UseSetting("DefaultCulture", "en");
                });
        }

        private WebApplicationFactory<BuyerWebProgram> CreateBuyerWebFactory(bool enforceDiscountCodes)
        {
            return new WebApplicationFactory<BuyerWebProgram>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseSetting("ASPNETCORE_HTTP_PORTS", "8080");
                    builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Development");
                    builder.UseSetting("RedisUrl", _webRedisUrl);
                    builder.UseSetting("ClientId", "663bba90-0036-4a58-8516-39faa8baba87");
                    builder.UseSetting("ClientSecret", "c61fcb32-cf9b-4cdd-84dc-4a1b173c36e9");
                    builder.UseSetting("OrderUrl", $"http://{_orderingApiContainer.Hostname}:{_orderingApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("ClientUrl", _clientApiUrl);
                    builder.UseSetting("GlobalUrl", _globalApiUrl);
                    builder.UseSetting("ProductColorAttributeId", TestProductColorAttributeId);
                    builder.UseSetting("CatalogUrl", $"http://{_catalogApiContainer.Hostname}:{_catalogApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("InventoryUrl", $"http://{_inventoryApiContainer.Hostname}:{_inventoryApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("BasketUrl", _basketApiUrl);
                    builder.UseSetting("IdentityUrl", $"http://{_mockAuthContainer.Hostname}:{_mockAuthContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("GrulaUrl", UnreachableGrulaUrl);
                    builder.UseSetting("GrulaAccessToken", TestGrulaAccessToken);
                    builder.UseSetting("GrulaEnvironmentId", TestGrulaEnvironmentId);
                    builder.UseSetting("DiscountCodeEnforcementEnabled", enforceDiscountCodes.ToString());
                    builder.UseSetting("IntegrationTestsEnabled", "true");
                    builder.UseSetting("SupportedCultures", "de,en,pl");
                    builder.UseSetting("DefaultCulture", "en");
                });
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

            await _globalApiContainer.StopAsync();
            await _globalApiContainer.DisposeAsync();

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
    }
}