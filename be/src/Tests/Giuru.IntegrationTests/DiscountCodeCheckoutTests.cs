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
using ClientRequestModel = Client.Api.v1.RequestModels.ClientRequestModel;
using DiscountCodeRequestModel = Seller.Web.Areas.Clients.ApiRequestModels.DiscountCodeRequestModel;
using SellerBasketApiResponseModel = Seller.Web.Areas.Orders.ApiResponseModels.BasketApiResponseModel;
using SellerBasketItemRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.BasketItemRequestModel;
using SellerBasketResponseModel = Seller.Web.Areas.Orders.ApiResponseModels.BasketResponseModel;
using SellerCheckoutBasketRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.CheckoutBasketRequestModel;
using SellerSaveBasketRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.SaveBasketRequestModel;

namespace Giuru.IntegrationTests
{
    // Checkout re-validates the Basket's Applied Discount Code, so a code withdrawn while it sat on a
    // Basket can never be ordered at its discounted price. Every refusal here leaves the Basket in
    // place, which is what lets the user save it again (dropping the code) and retry.
    [Collection(nameof(ApiCollection))]
    public class DiscountCodeCheckoutTests
    {
        private const string ClientsEndpoint = "api/v1/Clients";

        private readonly ApiFixture _apiFixture;

        public DiscountCodeCheckoutTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        [Fact]
        public async Task SellerCheckout_WhenTheAppliedCodeWasUnassignedFromTheClient_IsRefusedAndKeepsTheBasket()
        {
            var clientId = await CreateClientAsync();

            try
            {
                var code = NewCode();
                var codeId = await CreateCodeAsync(code, clientId);
                var basketId = Guid.NewGuid();
                await SaveSellerBasketAsync(basketId, clientId, code);
                await SaveCodeAsync(new DiscountCodeRequestModel { Id = codeId, Code = code, ClientIds = Array.Empty<Guid>() });

                var response = await _apiFixture.SellerWebClient.PostForResponseAsync(
                    ApiEndpoints.OrderCheckoutApiEndpoint,
                    new SellerCheckoutBasketRequestModel { BasketId = basketId, ClientId = clientId, ClientName = "Checkout client" });

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.Equal(code, (await GetPersistedBasketAsync(basketId)).DiscountCode);
            }
            finally
            {
                await DeleteClientAsync(clientId);
            }
        }

        [Fact]
        public async Task SellerCheckout_ForAnotherClientThanTheCodesOwner_IsRefusedAndKeepsTheBasket()
        {
            var ownerId = await CreateClientAsync();
            var otherId = await CreateClientAsync();

            try
            {
                var code = NewCode();
                await CreateCodeAsync(code, ownerId);
                var basketId = Guid.NewGuid();
                await SaveSellerBasketAsync(basketId, ownerId, code);

                var response = await _apiFixture.SellerWebClient.PostForResponseAsync(
                    ApiEndpoints.OrderCheckoutApiEndpoint,
                    new SellerCheckoutBasketRequestModel { BasketId = basketId, ClientId = otherId, ClientName = "Other client" });

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.Equal(code, (await GetPersistedBasketAsync(basketId)).DiscountCode);
            }
            finally
            {
                await DeleteClientAsync(ownerId);
                await DeleteClientAsync(otherId);
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
            var code = NewCode();
            var codeId = await CreateCodeAsync(code, client.Id.Value);

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
                            Sku = NewUnknownSku(),
                            Name = "Unpriced line",
                            Quantity = 1
                        }
                    }
                });
            Assert.Equal(code, saved.DiscountCode);
            await SaveCodeAsync(new DiscountCodeRequestModel { Id = codeId, Code = code, ClientIds = Array.Empty<Guid>() });

            var response = await _apiFixture.BuyerWebClient.PostForResponseAsync(
                ApiEndpoints.OrderCheckoutApiEndpoint,
                new BuyerCheckoutBasketRequestModel { BasketId = saved.Id, ClientId = client.Id, ClientName = "Checkout client" });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(code, (await GetPersistedBasketAsync(saved.Id.Value)).DiscountCode);
        }

        private static string NewCode()
        {
            return $"T{Guid.NewGuid():N}"[..16].ToUpperInvariant();
        }

        private static string NewUnknownSku()
        {
            return $"NO-PRODUCT-{Guid.NewGuid():N}";
        }

        private async Task<Guid> CreateCodeAsync(string code, params Guid[] clientIds)
        {
            var created = await SaveCodeAsync(new DiscountCodeRequestModel { Code = code, ClientIds = clientIds });

            return created.Id.Value;
        }

        private Task<BaseResponseModel> SaveCodeAsync(DiscountCodeRequestModel request)
        {
            return _apiFixture.SellerWebClient.PostAsync<DiscountCodeRequestModel, BaseResponseModel>(ApiEndpoints.DiscountCodesApiEndpoint, request);
        }

        private Task<SellerBasketResponseModel> SaveSellerBasketAsync(Guid basketId, Guid clientId, string discountCode)
        {
            return _apiFixture.SellerWebClient.PostAsync<SellerSaveBasketRequestModel, SellerBasketResponseModel>(
                ApiEndpoints.BasketApiEndpoint,
                new SellerSaveBasketRequestModel
                {
                    Id = basketId,
                    ClientId = clientId,
                    DiscountCode = discountCode,
                    Items = new List<SellerBasketItemRequestModel>
                    {
                        new SellerBasketItemRequestModel
                        {
                            ProductId = Guid.NewGuid(),
                            Sku = NewUnknownSku(),
                            Name = "Unpriced line",
                            Quantity = 1
                        }
                    }
                });
        }

        private Task<SellerBasketApiResponseModel> GetPersistedBasketAsync(Guid basketId)
        {
            return _apiFixture.BasketApiClient.GetAsync<SellerBasketApiResponseModel>($"{ApiConstants.Baskets.BasketsApiEndpoint}/{basketId}");
        }

        private async Task<Guid> CreateClientAsync()
        {
            var suffix = Guid.NewGuid().ToString("N");

            var created = await _apiFixture.ClientApiClient.PostAsync<ClientRequestModel, BaseResponseModel>(ClientsEndpoint, new ClientRequestModel
            {
                Name = $"Checkout-{suffix}",
                Email = $"{suffix}@discount-tests.com",
                CommunicationLanguage = Clients.Language,
                OrganisationId = Guid.NewGuid()
            });

            return created.Id.Value;
        }

        private async Task DeleteClientAsync(Guid clientId)
        {
            await _apiFixture.ClientApiClient.DeleteForResponseAsync($"{ClientsEndpoint}/{clientId}");
        }
    }
}
