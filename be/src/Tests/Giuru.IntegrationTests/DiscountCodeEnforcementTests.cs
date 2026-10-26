using Foundation.ApiExtensions.Models.Response;
using Giuru.IntegrationTests.Definitions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BuyerBasketItemRequestModel = Buyer.Web.Areas.Orders.ApiRequestModels.BasketItemRequestModel;
using BuyerSaveBasketRequestModel = Buyer.Web.Areas.Orders.ApiRequestModels.SaveBasketRequestModel;
using ClientRequestModel = Client.Api.v1.RequestModels.ClientRequestModel;
using DiscountCodeRequestModel = Seller.Web.Areas.Clients.ApiRequestModels.DiscountCodeRequestModel;
using SellerBasketItemRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.BasketItemRequestModel;
using SellerSaveBasketRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.SaveBasketRequestModel;

namespace Giuru.IntegrationTests
{
    // A new Discount Code is accepted on a Basket save only when it is a Valid Discount Code for the Client
    // concerned. Only the rejections are covered here: they are decided before pricing. The accepting and
    // dropping outcomes reach the pricing path, which needs a real catalog product, a real Client and Redis
    // on the host port; this fixture has none of them, so those outcomes are covered by unit tests.
    [Collection(nameof(ApiCollection))]
    public class DiscountCodeEnforcementTests
    {
        private const string ClientsEndpoint = "api/v1/Clients";

        private readonly ApiFixture _apiFixture;

        public DiscountCodeEnforcementTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        [Fact]
        public async Task SellerSaveBasket_WithUnknownCode_IsRejectedWithTheSameMessageAsAnUnassignedOne()
        {
            var code = $"T{Guid.NewGuid():N}"[..16].ToUpperInvariant();
            var suffix = Guid.NewGuid().ToString("N");

            var client = await _apiFixture.ClientApiClient.PostAsync<ClientRequestModel, BaseResponseModel>(ClientsEndpoint, new ClientRequestModel
            {
                Name = $"Enforce-{suffix}",
                Email = $"{suffix}@discount-tests.com",
                CommunicationLanguage = Clients.Language,
                OrganisationId = Guid.NewGuid()
            });

            try
            {
                // A code that exists but is assigned to nobody.
                await _apiFixture.SellerWebClient.PostAsync<DiscountCodeRequestModel, BaseResponseModel>(
                    ApiEndpoints.DiscountCodesApiEndpoint,
                    new DiscountCodeRequestModel { Code = code });

                var unassigned = await _apiFixture.SellerWebClient.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, CreateSellerRequest(client.Id.Value, code));
                var unknown = await _apiFixture.SellerWebClient.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, CreateSellerRequest(client.Id.Value, "UNKNOWN-CODE"));

                Assert.Equal(HttpStatusCode.BadRequest, unassigned.StatusCode);
                Assert.Equal(HttpStatusCode.BadRequest, unknown.StatusCode);
                Assert.Equal(await unassigned.Content.ReadAsStringAsync(), await unknown.Content.ReadAsStringAsync());
            }
            finally
            {
                await _apiFixture.ClientApiClient.DeleteForResponseAsync($"{ClientsEndpoint}/{client.Id}");
            }
        }

        [Fact]
        public async Task BuyerSaveBasket_WithCodeNobodyHolds_IsRejected()
        {
            var response = await _apiFixture.BuyerWebClient.PostForResponseAsync(
                ApiEndpoints.BasketApiEndpoint,
                new BuyerSaveBasketRequestModel
                {
                    DiscountCode = "UNKNOWN-CODE",
                    Items = new List<BuyerBasketItemRequestModel>
                    {
                        new BuyerBasketItemRequestModel
                        {
                            ProductId = Products.Lamica.Id,
                            Sku = Products.Lamica.Sku,
                            Name = Products.Lamica.Name,
                            Quantity = Inventories.Quantities.Quantity
                        }
                    }
                });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private static SellerSaveBasketRequestModel CreateSellerRequest(Guid clientId, string discountCode)
        {
            return new SellerSaveBasketRequestModel
            {
                Id = Guid.NewGuid(),
                ClientId = clientId,
                DiscountCode = discountCode,
                Items = new[]
                {
                    new SellerBasketItemRequestModel
                    {
                        ProductId = Products.Lamica.Id,
                        Sku = Products.Lamica.Sku,
                        Name = Products.Lamica.Name,
                        Quantity = 1
                    }
                }
            };
        }
    }
}
