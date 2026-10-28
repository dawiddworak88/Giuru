using Catalog.Api.v1.Products.RequestModels;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Shared.Definitions;
using Giuru.IntegrationTests.Definitions;
using Giuru.IntegrationTests.Helpers;
using Seller.Web.Areas.Orders.ApiRequestModels;
using Seller.Web.Areas.Orders.ApiResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientRequestModel = Client.Api.v1.RequestModels.ClientRequestModel;

namespace Giuru.IntegrationTests
{
    [Collection(nameof(ApiCollection))]
    public class BasketPricingTests
    {
        private const string ClientsEndpoint = "api/v1/Clients";

        private readonly ApiFixture _apiFixture;

        public BasketPricingTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        [Fact]
        public async Task SaveSellerBasket_WhenConfiguredGrulaIsUnavailable_ClearsUntrustedPricesBeforePersistence()
        {
            // Reaching the pricing path takes a product whose id matches the catalog's for the SKU and a
            // Client that exists in Client.Api, so the test creates both instead of using the fixed ids
            // in Definitions.
            var (productId, sku) = await CreateProductAsync();
            var clientId = await CreateClientAsync();

            try
            {
                await SaveBasketAndAssertPricesAreClearedAsync(productId, sku, clientId);
            }
            finally
            {
                await _apiFixture.ClientApiClient.DeleteForResponseAsync($"{ClientsEndpoint}/{clientId}");
            }
        }

        private async Task SaveBasketAndAssertPricesAreClearedAsync(Guid productId, string sku, Guid clientId)
        {
            var basketId = Guid.NewGuid();

            var savedBasket = await _apiFixture.SellerWebClient.PostAsync<SaveBasketRequestModel, BasketResponseModel>(
                ApiEndpoints.BasketApiEndpoint,
                new SaveBasketRequestModel
                {
                    Id = basketId,
                    ClientId = clientId,
                    Items = new List<BasketItemRequestModel>
                    {
                        new()
                        {
                            ProductId = productId,
                            Sku = sku,
                            Name = $"Unpriced {sku}",
                            Quantity = Inventories.Quantities.Quantity,
                            UnitPrice = 0.01m,
                            Price = 0.01m,
                            Currency = "FAKE"
                        },
                        new()
                        {
                            ProductId = productId,
                            Sku = sku,
                            Name = $"Unpriced {sku} duplicate line",
                            Quantity = 2,
                            UnitPrice = 999m,
                            Price = 1998m,
                            Currency = "USD"
                        }
                    }
                });

            Assert.Equal(basketId, savedBasket.Id);
            AssertPricesAreClearedInSellerResponse(savedBasket.Items);

            // Basket API persists prices supplied only by the public web applications;
            // reading it here proves Seller cleared them before crossing that boundary.
            var persistedBasket = await _apiFixture.BasketApiClient.GetAsync<BasketApiResponseModel>(
                $"{ApiConstants.Baskets.BasketsApiEndpoint}/{basketId}");
            AssertPricesAreClearedInPersistedBasket(persistedBasket.Items);
        }

        private async Task<(Guid Id, string Sku)> CreateProductAsync()
        {
            var sku = $"GRULA-{Guid.NewGuid():N}"[..20];

            var productId = await InventoryHelper.CreateProductAndAddToStockAsync(_apiFixture, new ProductRequestModel
            {
                Name = $"Unpriced {sku}",
                Sku = sku,
                CategoryId = Products.Lamica.CategoryId,
                IsPublished = true
            });

            return (productId.Value, sku);
        }

        private async Task<Guid> CreateClientAsync()
        {
            var suffix = Guid.NewGuid().ToString("N");

            var created = await _apiFixture.ClientApiClient.PostAsync<ClientRequestModel, BaseResponseModel>(ClientsEndpoint, new ClientRequestModel
            {
                Name = $"Pricing-{suffix}",
                Email = $"{suffix}@pricing-tests.com",
                CommunicationLanguage = Clients.Language,
                OrganisationId = Guid.NewGuid()
            });

            return created.Id.Value;
        }

        private static void AssertPricesAreClearedInSellerResponse(IEnumerable<BasketItemResponseModel> items)
        {
            Assert.NotNull(items);
            Assert.All(items, item =>
            {
                Assert.Null(item.UnitPrice);
                Assert.Null(item.Price);
                Assert.Null(item.Currency);
            });
        }

        private static void AssertPricesAreClearedInPersistedBasket(IEnumerable<BasketItemApiResponseModel> items)
        {
            Assert.NotNull(items);
            Assert.All(items, item =>
            {
                Assert.Null(item.UnitPrice);
                Assert.Null(item.Price);
                Assert.Null(item.Currency);
            });
        }
    }
}