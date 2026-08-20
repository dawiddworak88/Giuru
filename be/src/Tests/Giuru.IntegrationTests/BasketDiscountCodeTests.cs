using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using Foundation.ApiExtensions.Shared.Definitions;
using Giuru.IntegrationTests.Definitions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
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
        public async Task SaveBuyerBasket_WithDiscountCode_WhenConfiguredGrulaIsUnavailable_StillStoresTheCode()
        {
            // Discount-code storage is gated purely on IsGrulaConfigured, not on Grula's
            // reachability (see BasketsApiController.Index) - only pricing is cleared when
            // the configured Grula instance can't be reached (see BasketPricingTests).
            var basket = await _apiFixture.BuyerWebClient.PostAsync<SaveBasketRequestModel, BasketResponseModel>(
                ApiEndpoints.BasketApiEndpoint,
                new SaveBasketRequestModel
                {
                    DiscountCode = DiscountCode,
                    Items = new List<BasketItemRequestModel>
                    {
                        new BasketItemRequestModel
                        {
                            ProductId = Products.Lamica.Id,
                            Sku = Products.Lamica.Sku,
                            Name = Products.Lamica.Name,
                            Quantity = Inventories.Quantities.Quantity
                        }
                    }
                });

            Assert.NotNull(basket);
            Assert.Equal(DiscountCode, basket.DiscountCode);
        }
    }
}
