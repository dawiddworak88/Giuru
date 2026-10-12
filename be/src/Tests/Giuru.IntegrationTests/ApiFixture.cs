using Catalog.Api.v1.Products.RequestModels;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Giuru.IntegrationTests.Definitions;
using Giuru.IntegrationTests.Helpers;
using Giuru.IntegrationTests.HttpClients;
using Giuru.IntegrationTests.Images;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
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

        // Repricing a basket line that resolves to a real product translates its colours first, which reads the
        // items of this attribute from the catalog. No such attribute exists here, so the answer is an empty list.
        private const string TestProductColorAttributeId = "00000000-0000-0000-0000-000000000002";

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

        private WebApplicationFactory<SellerWebProgram> _sellerWebFactory;
        private WebApplicationFactory<BuyerWebProgram> _buyerWebFactory;
        private string _tokenEndpoint;

        public RestClient SellerWebClient { get; private set; }
        public RestClient BuyerWebClient { get; private set; }
        public RestClient BasketApiClient { get; private set; }

        /// <summary>Client.Api, signed in as the seller the whole run signs in as.</summary>
        public RestClient ClientApiClient { get; private set; }

        /// <summary>The organisation of the seller every default client signs in as.</summary>
        public static readonly Guid SellerOrganisationId = Guid.Parse("09affcc9-1665-45d6-919f-3d2026106ba1");

        /// <summary>The organisation of a second seller, who must never see or touch the first one's data.</summary>
        public static readonly Guid OtherSellerOrganisationId = Guid.Parse("f3c1f7a2-6b1e-4d6a-9d52-5b8e7a0c4d11");

        public const string SellerEmail = "seller@user.com";
        public const string OtherSellerEmail = "other.seller@user.com";

        /// <summary>
        /// Issues a token for an identity of the test's choosing. An empty <paramref name="role"/> issues a token without
        /// any role, which is what a buyer or a member of a buyer's team looks like.
        /// </summary>
        public async Task<string> IssueTokenAsync(string email, string role, Guid organisationId)
        {
            var query = $"?email={Uri.EscapeDataString(email)}&role={Uri.EscapeDataString(string.IsNullOrWhiteSpace(role) ? "none" : role)}&organisationId={organisationId}";

            return await new TokenClient(new HttpClient()).GetTokenAsync(_tokenEndpoint + query);
        }

        public Task<string> IssueSellerTokenAsync(string email, Guid organisationId)
        {
            return IssueTokenAsync(email, "Seller", organisationId);
        }

        public Task<string> IssueBuyerTokenAsync(string email, Guid organisationId)
        {
            return IssueTokenAsync(email, string.Empty, organisationId);
        }

        /// <summary>
        /// Each client has a cookie jar of its own, so a buyer web client keeps one basket across its requests and
        /// a new client starts with a new basket.
        /// </summary>
        public RestClient CreateSellerWebClient(string token) => new RestClient(CreateAuthenticatedClient(_sellerWebFactory.CreateClient(), token));

        public RestClient CreateBuyerWebClient(string token) => new RestClient(CreateAuthenticatedClient(_buyerWebFactory.CreateClient(), token));

        public RestClient CreateClientApiClient(string token)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri($"http://{_clientApiContainer.Hostname}:{_clientApiContainer.GetMappedPublicPort(8080)}")
            };

            return new RestClient(CreateAuthenticatedClient(httpClient, token));
        }

        private readonly SemaphoreSlim _discountCodeProductLock = new SemaphoreSlim(1, 1);
        private DiscountCodeProduct _discountCodeProduct;

        /// <summary>
        /// A product of its own for the discount code tests, created once for the whole run. Its identity has to be the
        /// catalog's, because a basket line whose product id does not match its SKU is refused when it is repriced.
        /// </summary>
        public async Task<DiscountCodeProduct> GetDiscountCodeProductAsync()
        {
            await _discountCodeProductLock.WaitAsync();

            try
            {
                if (_discountCodeProduct is null)
                {
                    var productId = await InventoryHelper.CreateProductAndAddToStockAsync(this, new ProductRequestModel
                    {
                        Name = Products.DiscountCodeProduct.Name,
                        Sku = Products.DiscountCodeProduct.Sku,
                        CategoryId = Products.DiscountCodeProduct.CategoryId,
                        IsPublished = Products.DiscountCodeProduct.IsPublished,
                        Ean = Products.DiscountCodeProduct.Ean
                    });

                    _discountCodeProduct = new DiscountCodeProduct(productId.Value, Products.DiscountCodeProduct.Sku, Products.DiscountCodeProduct.Name);
                }

                return _discountCodeProduct;
            }
            finally
            {
                _discountCodeProductLock.Release();
            }
        }

        private static HttpClient CreateAuthenticatedClient(HttpClient httpClient, string token)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return httpClient;
        }

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

            _tokenEndpoint = $"http://{_mockAuthContainer.Hostname}:{_mockAuthContainer.GetMappedPublicPort(8080)}/api/token";

            var tokenClient = new TokenClient(new HttpClient());
            var token = await tokenClient.GetTokenAsync(_tokenEndpoint);

            var basketApiHttpClient = new HttpClient
            {
                BaseAddress = new Uri($"http://{_basketApiContainer.Hostname}:{_basketApiContainer.GetMappedPublicPort(8080)}")
            };

            basketApiHttpClient.DefaultRequestHeaders.Accept.Clear();
            basketApiHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            basketApiHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            BasketApiClient = new RestClient(basketApiHttpClient);

            _sellerWebFactory = new WebApplicationFactory<SellerWebProgram>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseSetting("ASPNETCORE_HTTP_PORTS", "8080");
                    builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Development");
                    // The seller app reads the colour translations it prices with from Redis itself, so like the buyer app
                    // it needs an address the test process can reach rather than the container network's name.
                    builder.UseSetting("RedisUrl", $"{_redisContainer.Hostname}:{_redisContainer.GetMappedPublicPort(6379)},abortConnect=false");
                    builder.UseSetting("ClientId", "663bba90-0036-4a58-8516-39faa8baba87");
                    builder.UseSetting("ClientSecret", "c61fcb32-cf9b-4cdd-84dc-4a1b173c36e9");
                    builder.UseSetting("ClientUrl", $"http://{_clientApiContainer.Hostname}:{_clientApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("CatalogUrl", $"http://{_catalogApiContainer.Hostname}:{_catalogApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("InventoryUrl", $"http://{_inventoryApiContainer.Hostname}:{_inventoryApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("BasketUrl", $"http://{_basketApiContainer.Hostname}:{_basketApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("IdentityUrl", $"http://{_mockAuthContainer.Hostname}:{_mockAuthContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("GlobalUrl", $"http://{_mockAuthContainer.Hostname}:{_mockAuthContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("GrulaUrl", UnreachableGrulaUrl);
                    builder.UseSetting("GrulaAccessToken", TestGrulaAccessToken);
                    builder.UseSetting("GrulaEnvironmentId", TestGrulaEnvironmentId);
                    builder.UseSetting("ProductColorAttributeId", TestProductColorAttributeId);
                    builder.UseSetting("IntegrationTestsEnabled", "true");
                    builder.UseSetting("SupportedCultures", "de,en,pl");
                    builder.UseSetting("DefaultCulture", "en");
                });

            var sellerWebFactpry = _sellerWebFactory.CreateClient();

            sellerWebFactpry.DefaultRequestHeaders.Accept.Clear();
            sellerWebFactpry.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            sellerWebFactpry.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            SellerWebClient = new RestClient(sellerWebFactpry);

            _buyerWebFactory = new WebApplicationFactory<BuyerWebProgram>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseSetting("ASPNETCORE_HTTP_PORTS", "8080");
                    builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Development");
                    // The buyer app caches claims and reference data in Redis itself, so it needs an address the test process can reach.
                    builder.UseSetting("RedisUrl", $"{_redisContainer.Hostname}:{_redisContainer.GetMappedPublicPort(6379)},abortConnect=false");
                    builder.UseSetting("ClientId", "663bba90-0036-4a58-8516-39faa8baba87");
                    builder.UseSetting("ClientSecret", "c61fcb32-cf9b-4cdd-84dc-4a1b173c36e9");
                    builder.UseSetting("OrderUrl", $"http://{_orderingApiContainer.Hostname}:{_orderingApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("ClientUrl", $"http://{_clientApiContainer.Hostname}:{_clientApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("CatalogUrl", $"http://{_catalogApiContainer.Hostname}:{_catalogApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("InventoryUrl", $"http://{_inventoryApiContainer.Hostname}:{_inventoryApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("BasketUrl", $"http://{_basketApiContainer.Hostname}:{_basketApiContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("IdentityUrl", $"http://{_mockAuthContainer.Hostname}:{_mockAuthContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("GlobalUrl", $"http://{_mockAuthContainer.Hostname}:{_mockAuthContainer.GetMappedPublicPort(8080)}");
                    builder.UseSetting("GrulaUrl", UnreachableGrulaUrl);
                    builder.UseSetting("GrulaAccessToken", TestGrulaAccessToken);
                    builder.UseSetting("GrulaEnvironmentId", TestGrulaEnvironmentId);
                    builder.UseSetting("ProductColorAttributeId", TestProductColorAttributeId);
                    builder.UseSetting("IntegrationTestsEnabled", "true");
                    builder.UseSetting("SupportedCultures", "de,en,pl");
                    builder.UseSetting("DefaultCulture", "en");
                });

            var buyerWebFactpry = _buyerWebFactory.CreateClient();

            buyerWebFactpry.DefaultRequestHeaders.Accept.Clear();
            buyerWebFactpry.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            buyerWebFactpry.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            BuyerWebClient = new RestClient(buyerWebFactpry);

            ClientApiClient = CreateClientApiClient(token);
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
    }
}