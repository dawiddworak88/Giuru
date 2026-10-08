using Foundation.ApiExtensions.Shared.Definitions;
using Giuru.IntegrationTests.Definitions;
using Giuru.IntegrationTests.Helpers;
using Seller.Web.Areas.Orders.ApiRequestModels;
using Seller.Web.Areas.Orders.ApiResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
    [Collection(nameof(ApiCollection))]
    public class BasketPricingTests
    {
        private readonly ApiFixture _apiFixture;

        public BasketPricingTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        [Fact]
        public async Task SaveSellerBasket_WhenConfiguredGrulaIsUnavailable_ClearsUntrustedPricesBeforePersistence()
        {
            var basketId = Guid.NewGuid();

            // Pricing resolves the client the basket is priced for, so it has to exist and belong to the calling seller.
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var client = await seller.CreateClientAsync();

            var savedBasket = await _apiFixture.CreateSellerWebClient(seller.Token).PostAsync<SaveBasketRequestModel, BasketResponseModel>(
                ApiEndpoints.BasketApiEndpoint,
                new SaveBasketRequestModel
                {
                    Id = basketId,
                    ClientId = client.Id,
                    Items = new List<BasketItemRequestModel>
                    {
                        new()
                        {
                            ProductId = Products.Lamica.Id,
                            Sku = Products.Lamica.Sku,
                            Name = Products.Lamica.Name,
                            Quantity = Inventories.Quantities.Quantity,
                            UnitPrice = 0.01m,
                            Price = 0.01m,
                            Currency = "FAKE"
                        },
                        new()
                        {
                            ProductId = Products.Lamica.Id,
                            Sku = Products.Lamica.Sku,
                            Name = $"{Products.Lamica.Name} duplicate line",
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