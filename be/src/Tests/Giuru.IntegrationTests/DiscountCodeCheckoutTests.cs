using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Shared.Definitions;
using Giuru.IntegrationTests.Definitions;
using Giuru.IntegrationTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BuyerBasketItemRequestModel = Buyer.Web.Areas.Orders.ApiRequestModels.BasketItemRequestModel;
using BuyerBasketResponseModel = Buyer.Web.Areas.Orders.ApiResponseModels.BasketResponseModel;
using BuyerCheckoutBasketRequestModel = Buyer.Web.Areas.Orders.ApiRequestModels.CheckoutBasketRequestModel;
using BuyerSaveBasketRequestModel = Buyer.Web.Areas.Orders.ApiRequestModels.SaveBasketRequestModel;
using DiscountCodeRequestModel = Seller.Web.Areas.Clients.ApiRequestModels.DiscountCodeRequestModel;
using SellerCheckoutBasketRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.CheckoutBasketRequestModel;

namespace Giuru.IntegrationTests
{
    // Checkout re-validates the Basket's Applied Discount Code, so a code withdrawn while it sat on a
    // Basket can never be ordered at its discounted price. Every refusal here leaves the Basket in
    // place, which is what lets the user save it again (dropping the code) and retry.
    [Collection(nameof(ApiCollection))]
    public class DiscountCodeCheckoutTests
    {
        private readonly ApiFixture _apiFixture;

        public DiscountCodeCheckoutTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        [Fact]
        public async Task SellerCheckout_WhenTheAppliedCodeWasUnassignedFromTheClient_IsRefusedAndKeepsTheBasket()
        {
            var clientId = await ClientHelper.CreateClientAsync(_apiFixture, "Checkout");

            try
            {
                var code = DiscountCodeHelper.NewCode();
                var codeId = await DiscountCodeHelper.CreateCodeAsync(_apiFixture, code, clientId);
                var basketId = Guid.NewGuid();
                await DiscountCodeHelper.SaveSellerBasketAsync(_apiFixture, basketId, clientId, code);
                await DiscountCodeHelper.SaveCodeAsync(_apiFixture, new DiscountCodeRequestModel { Id = codeId, Code = code, ClientIds = Array.Empty<Guid>() });

                var response = await _apiFixture.SellerWebClient.PostForResponseAsync(
                    ApiEndpoints.OrderCheckoutApiEndpoint,
                    new SellerCheckoutBasketRequestModel { BasketId = basketId, ClientId = clientId, ClientName = "Checkout client" });

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.Equal(code, (await DiscountCodeHelper.GetPersistedBasketAsync(_apiFixture, basketId)).DiscountCode);
            }
            finally
            {
                await ClientHelper.DeleteClientAsync(_apiFixture, clientId);
            }
        }

        [Fact]
        public async Task SellerCheckout_ForAnotherClientThanTheCodesOwner_IsRefusedAndKeepsTheBasket()
        {
            var ownerId = await ClientHelper.CreateClientAsync(_apiFixture, "Checkout");
            var otherId = await ClientHelper.CreateClientAsync(_apiFixture, "Checkout");

            try
            {
                var code = DiscountCodeHelper.NewCode();
                await DiscountCodeHelper.CreateCodeAsync(_apiFixture, code, ownerId);
                var basketId = Guid.NewGuid();
                await DiscountCodeHelper.SaveSellerBasketAsync(_apiFixture, basketId, ownerId, code);

                var response = await _apiFixture.SellerWebClient.PostForResponseAsync(
                    ApiEndpoints.OrderCheckoutApiEndpoint,
                    new SellerCheckoutBasketRequestModel { BasketId = basketId, ClientId = otherId, ClientName = "Other client" });

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.Equal(code, (await DiscountCodeHelper.GetPersistedBasketAsync(_apiFixture, basketId)).DiscountCode);
            }
            finally
            {
                await ClientHelper.DeleteClientAsync(_apiFixture, ownerId);
                await ClientHelper.DeleteClientAsync(_apiFixture, otherId);
            }
        }

        [Fact]
        public async Task BuyerCheckout_WithAClientIdOtherThanTheSignedInClient_IsRefused()
        {
            var response = await _apiFixture.BuyerWebClient.PostForResponseAsync(
                ApiEndpoints.OrderCheckoutApiEndpoint,
                new BuyerCheckoutBasketRequestModel { BasketId = Guid.NewGuid(), ClientId = Guid.NewGuid(), ClientName = "Somebody else" });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task BuyerCheckout_WhenTheAppliedCodeWasUnassignedFromTheClient_IsRefused()
        {
            var client = await ClientHelper.GetOrCreateTestUsersClientAsync(_apiFixture);
            var code = DiscountCodeHelper.NewCode();
            var codeId = await DiscountCodeHelper.CreateCodeAsync(_apiFixture, code, client.Id.Value);

            var saved = await _apiFixture.BuyerWebClient.PostAsync<BuyerSaveBasketRequestModel, BuyerBasketResponseModel>(
                ApiEndpoints.BasketApiEndpoint,
                new BuyerSaveBasketRequestModel
                {
                    DiscountCode = code,
                    Items = new List<BuyerBasketItemRequestModel>
                    {
                        new BuyerBasketItemRequestModel
                        {
                            ProductId = Guid.NewGuid(),
                            Sku = DiscountCodeHelper.NewUnknownSku(),
                            Name = "Unpriced line",
                            Quantity = 1
                        }
                    }
                });
            Assert.Equal(code, saved.DiscountCode);
            await DiscountCodeHelper.SaveCodeAsync(_apiFixture, new DiscountCodeRequestModel { Id = codeId, Code = code, ClientIds = Array.Empty<Guid>() });

            var response = await _apiFixture.BuyerWebClient.PostForResponseAsync(
                ApiEndpoints.OrderCheckoutApiEndpoint,
                new BuyerCheckoutBasketRequestModel { BasketId = saved.Id, ClientId = client.Id, ClientName = "Checkout client" });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(code, (await DiscountCodeHelper.GetPersistedBasketAsync(_apiFixture, saved.Id.Value)).DiscountCode);
        }
    }
}
