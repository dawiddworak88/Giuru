using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using Foundation.ApiExtensions.Shared.Definitions;
using Giuru.IntegrationTests.Definitions;
using Giuru.IntegrationTests.Helpers;
using Giuru.IntegrationTests.HttpClients;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
    // The tests that go straight to Basket.Api keep their meaning: that service stores whatever code it is given. The rule
    // that a code must exist, be active and be assigned to the client is enforced by the buyer and seller apps in front
    // of it (see BuyerDiscountCodeEnforcementTests and SellerDiscountCodeEnforcementTests), so a direct call to
    // Basket.Api is outside that enforcement boundary by decision.
    [Collection(nameof(ApiCollection))]
    public class BasketDiscountCodeTests
    {
        private const string DiscountCode = "SUMMER25";
        private const string OtherDiscountCode = "WINTER25";

        private readonly ApiFixture _apiFixture;

        public BasketDiscountCodeTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        private static SaveBasketApiRequestModel CreateBasketApiRequest(Guid basketId, string discountCode)
        {
            return new SaveBasketApiRequestModel
            {
                Id = basketId,
                DiscountCode = discountCode,
                Items = new List<BasketItemApiRequestModel>
                {
                    new BasketItemApiRequestModel
                    {
                        ProductId = Products.Lamica.Id,
                        ProductSku = Products.Lamica.Sku,
                        ProductName = Products.Lamica.Name,
                        Quantity = Inventories.Quantities.Quantity
                    }
                }
            };
        }

        private Task<BasketApiResponseModel> SaveBasketAsync(Guid basketId, string discountCode)
        {
            return _apiFixture.BasketApiClient.PostAsync<SaveBasketApiRequestModel, BasketApiResponseModel>(
                ApiConstants.Baskets.BasketsApiEndpoint,
                CreateBasketApiRequest(basketId, discountCode));
        }

        private Task<BasketApiResponseModel> GetBasketAsync(Guid basketId)
        {
            return _apiFixture.BasketApiClient.GetAsync<BasketApiResponseModel>(
                $"{ApiConstants.Baskets.BasketsApiEndpoint}/{basketId}");
        }

        [Fact]
        public async Task SaveBasket_WithDiscountCode_StoresItAndReturnsItOnRead()
        {
            var basketId = Guid.NewGuid();

            var saved = await SaveBasketAsync(basketId, DiscountCode);

            Assert.Equal(basketId, saved.Id);
            Assert.Equal(DiscountCode, saved.DiscountCode);

            var read = await GetBasketAsync(basketId);

            Assert.Equal(DiscountCode, read.DiscountCode);
        }

        [Fact]
        public async Task SaveBasket_WithAnotherDiscountCode_ReplacesTheStoredOne()
        {
            var basketId = Guid.NewGuid();

            await SaveBasketAsync(basketId, DiscountCode);

            var saved = await SaveBasketAsync(basketId, OtherDiscountCode);

            Assert.Equal(OtherDiscountCode, saved.DiscountCode);
            Assert.Equal(OtherDiscountCode, (await GetBasketAsync(basketId)).DiscountCode);
        }

        [Fact]
        public async Task SaveBasket_WithoutDiscountCode_ClearsTheStoredOne()
        {
            var basketId = Guid.NewGuid();

            await SaveBasketAsync(basketId, DiscountCode);

            // The basket service is last-write-wins and has no notion of "leave the code
            // alone" - that is exactly why the web layer resolves the code before saving.
            var saved = await SaveBasketAsync(basketId, null);

            Assert.Null(saved.DiscountCode);
            Assert.Null((await GetBasketAsync(basketId)).DiscountCode);
        }

        [Fact]
        public async Task SaveBasket_WithDiscountCodeLongerThanAllowed_IsRejected()
        {
            var response = await _apiFixture.BasketApiClient.PostForResponseAsync(
                ApiConstants.Baskets.BasketsApiEndpoint,
                CreateBasketApiRequest(Guid.NewGuid(), new string('A', 65)));

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task CheckoutBasket_WhenTheBasketNoLongerExists_IsRejectedInsteadOfPlacingAnEmptyOrder()
        {
            // The checkout reads the basket for its discount code and its lines, so a basket
            // that expired or was already checked out must not turn into an empty order.
            var response = await _apiFixture.BasketApiClient.PostForResponseAsync(
                ApiConstants.Baskets.BasketsCheckoutApiEndpoint,
                new CheckoutBasketApiRequestModel
                {
                    BasketId = Guid.NewGuid(),
                    ClientId = Clients.Id,
                    ClientName = Clients.Name,
                    ClientEmail = Clients.Email,
                    SellerId = Clients.OrganisationId
                });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task SaveBasket_DirectlyAtBasketApi_StoresACodeNobodyRegistered()
        {
            // Direct Basket.Api calls are outside the enforcement boundary: only the web apps check a code.
            var saved = await SaveBasketAsync(Guid.NewGuid(), $"UNREGISTERED{Guid.NewGuid():N}".Substring(0, 24));

            Assert.False(string.IsNullOrWhiteSpace(saved.DiscountCode));
        }

        private async Task<(DiscountCodeKit Kit, TestClient Client, RestClient Web, DiscountCodeProduct Product)> CreateBuyerAsync()
        {
            var kit = await DiscountCodeKit.CreateAsync(_apiFixture);
            var client = await kit.CreateClientAsync();

            return (kit, client, await kit.CreateBuyerWebClientAsync(client), await _apiFixture.GetDiscountCodeProductAsync());
        }

        private static SaveBasketRequestModel BuyerBasket(DiscountCodeProduct product, string discountCode)
        {
            return new SaveBasketRequestModel
            {
                DiscountCode = discountCode,
                Items = new List<BasketItemRequestModel>
                {
                    new BasketItemRequestModel
                    {
                        ProductId = product.Id,
                        Sku = product.Sku,
                        Name = product.Name,
                        Quantity = Inventories.Quantities.Quantity
                    }
                }
            };
        }

        [Fact]
        public async Task SaveBuyerBasket_WithAnUnregisteredNewDiscountCode_IsRejected()
        {
            var buyer = await CreateBuyerAsync();

            var response = await buyer.Web.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, BuyerBasket(buyer.Product, DiscountCodeKit.NewCode()));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("This discount code cannot be applied", await RestClient.ReadMessageAsync(response));
        }

        [Fact]
        public async Task SaveBuyerBasket_WithARegisteredAssignedDiscountCode_WhenConfiguredGrulaIsUnavailable_StillStoresTheCode()
        {
            // Discount-code storage is gated purely on IsGrulaConfigured, not on Grula's reachability (see
            // BasketsApiController.Index) - only pricing is cleared when the configured Grula instance can't be reached
            // (see BasketPricingTests). Whether the code may be applied at all is decided by Client.Api.
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });

            var basket = await buyer.Web.PostAsync<SaveBasketRequestModel, BasketResponseModel>(ApiEndpoints.BasketApiEndpoint, BuyerBasket(buyer.Product, codeText));

            Assert.NotNull(basket);
            Assert.Equal(codeText, basket.DiscountCode);
        }
    }
}
