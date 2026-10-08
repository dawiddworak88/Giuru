using Buyer.Web.Areas.Orders.ApiResponseModels;
using Foundation.ApiExtensions.Shared.Definitions;
using Giuru.IntegrationTests.Definitions;
using Giuru.IntegrationTests.Helpers;
using Giuru.IntegrationTests.HttpClients;
using Seller.Web.Areas.Clients.ApiRequestModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SellerBasketResponseModel = Seller.Web.Areas.Orders.ApiResponseModels.BasketResponseModel;
using SellerCheckoutBasketRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.CheckoutBasketRequestModel;

namespace Giuru.IntegrationTests
{
    /// <summary>
    /// Seller.Web with discount code enforcement on: a code is used only when it exists and is assigned to the client the
    /// seller works for. Codes and assignments are seeded through Client.Api, because Grula is unreachable here.
    /// </summary>
    [Collection(nameof(ApiCollection))]
    public class SellerDiscountCodeEnforcementTests
    {
        private const string Code = "SUMMER25";

        private readonly ApiFixture _apiFixture;

        public SellerDiscountCodeEnforcementTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        private static Dictionary<string, object> BasketRequest(Guid clientId, Guid? basketId = null, string discountCode = null, bool includeDiscountCode = true)
        {
            var request = new Dictionary<string, object>
            {
                ["clientId"] = clientId,
                ["items"] = new[]
                {
                    new Dictionary<string, object>
                    {
                        ["productId"] = Products.Lamica.Id,
                        ["sku"] = Products.Lamica.Sku,
                        ["name"] = Products.Lamica.Name,
                        ["quantity"] = Inventories.Quantities.Quantity
                    }
                }
            };

            if (basketId.HasValue)
            {
                request["id"] = basketId.Value;
            }

            // An omitted code means "leave it alone"; an explicit null removes it.
            if (includeDiscountCode)
            {
                request["discountCode"] = discountCode;
            }

            return request;
        }

        private static Task<HttpResponseMessage> SaveAsync(RestClient web, Dictionary<string, object> request)
        {
            return web.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, request);
        }

        private static async Task<SellerBasketResponseModel> SaveOkAsync(RestClient web, Dictionary<string, object> request)
        {
            var response = await SaveAsync(web, request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            return await RestClient.ReadAsync<SellerBasketResponseModel>(response);
        }

        private Task<BasketApiResponseModel> GetStoredBasketAsync(Guid basketId)
        {
            return _apiFixture.BasketApiClient.GetAsync<BasketApiResponseModel>($"{ApiConstants.Baskets.BasketsApiEndpoint}/{basketId}");
        }

        private static Task<HttpResponseMessage> CheckoutAsync(RestClient web, Guid basketId, DiscountClient client)
        {
            return web.PostForResponseAsync(
                ApiEndpoints.OrderCheckoutApiEndpoint,
                new SellerCheckoutBasketRequestModel
                {
                    BasketId = basketId,
                    ClientId = client.Id,
                    ClientName = client.Email
                });
        }

        private static async Task<string> MessageAsync(HttpResponseMessage response)
        {
            return (await RestClient.ReadAsync<MessageResponse>(response))?.Message;
        }

        private sealed class MessageResponse
        {
            public string Message { get; set; }
        }

        // ---- Managing codes in Seller.Web ----

        [Fact]
        public async Task CreateCode_WhenGrulaCannotBeReached_IsRefusedAndNothingIsStored()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var web = seller.CreateEnforcedSellerWebClient();

            var response = await web.PostForResponseAsync(ApiEndpoints.DiscountCodesApiEndpoint, new DiscountCodeRequestModel { Code = Code });

            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
            Assert.False(string.IsNullOrWhiteSpace(await MessageAsync(response)));
            Assert.Empty(await seller.ListCodesAsync());
        }

        [Fact]
        public async Task UpdateCode_DoesNotNeedGrula()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var web = seller.CreateEnforcedSellerWebClient();

            var response = await web.PostForResponseAsync(
                ApiEndpoints.DiscountCodesApiEndpoint,
                new DiscountCodeRequestModel { Id = codeId, Description = "Edited", IsDisabled = true });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var stored = Assert.Single(await seller.ListCodesAsync());
            Assert.Equal("Edited", stored.Description);
            Assert.True(stored.IsDisabled);
        }

        [Fact]
        public async Task ManageCodes_ThroughSellerWeb_IsForSellersOnly()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var client = await seller.CreateClientAsync();
            var buyerWeb = _apiFixture.CreateEnforcedSellerWebClient(await client.GetBuyerTokenAsync());

            var list = await buyerWeb.GetForResponseAsync($"{ApiEndpoints.GetDiscountCodesApiEndpoint}?pageIndex=1&itemsPerPage=10");
            var create = await buyerWeb.PostForResponseAsync(ApiEndpoints.DiscountCodesApiEndpoint, new DiscountCodeRequestModel { Code = Code });

            Assert.Equal(HttpStatusCode.Forbidden, list.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, create.StatusCode);
        }

        [Fact]
        public async Task ManageCodes_WhenTheFeatureIsEnabledForAnotherSeller_StaysIsolated()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var other = await DiscountSeller.CreateAsync(_apiFixture);
            await seller.CreateCodeAsync(Code);

            var response = await other.CreateEnforcedSellerWebClient().GetForResponseAsync($"{ApiEndpoints.GetDiscountCodesApiEndpoint}?pageIndex=1&itemsPerPage=10");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.DoesNotContain(Code, await response.Content.ReadAsStringAsync());
        }

        // ---- Writing a basket ----

        [Fact]
        public async Task SaveBasket_WithAnAssignedCode_StoresTheCanonicalSpellingAndTheValidatedClient()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var web = seller.CreateEnforcedSellerWebClient();

            var saved = await SaveOkAsync(web, BasketRequest(client.Id, discountCode: "summer25"));

            Assert.Equal(Code, saved.DiscountCode);
            Assert.Null(saved.DiscountCodeRemovedMessage);

            var stored = await GetStoredBasketAsync(saved.Id.Value);
            Assert.Equal(Code, stored.DiscountCode);

            // Set by the web tier from the validated client, never taken from the browser.
            Assert.Equal(client.Id, stored.DiscountCodeClientId);
            Assert.NotNull(stored.BasketVersion);
        }

        [Fact]
        public async Task SaveBasket_WithACodeFromTheBrowserClaimingAnotherClient_StillStoresTheValidatedClient()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var web = seller.CreateEnforcedSellerWebClient();

            var request = BasketRequest(client.Id, discountCode: Code);
            request["discountCodeClientId"] = Guid.NewGuid();

            var saved = await SaveOkAsync(web, request);

            Assert.Equal(client.Id, (await GetStoredBasketAsync(saved.Id.Value)).DiscountCodeClientId);
        }

        [Fact]
        public async Task SaveBasket_WithACodeThatIsNotAssignedToTheClient_Returns400WithTheSpecificReasonAndLeavesTheBasketUntouched()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            await seller.CreateCodeAsync("UNASSIGNED");
            var client = await seller.CreateClientAsync();
            var web = seller.CreateEnforcedSellerWebClient();

            var existing = await SaveOkAsync(web, BasketRequest(client.Id, includeDiscountCode: false));
            var before = await GetStoredBasketAsync(existing.Id.Value);

            var response = await SaveAsync(web, BasketRequest(client.Id, existing.Id, "UNASSIGNED"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var after = await GetStoredBasketAsync(existing.Id.Value);
            Assert.Null(after.DiscountCode);
            Assert.Equal(before.BasketVersion, after.BasketVersion);
        }

        [Fact]
        public async Task SaveBasket_GivesSellersADifferentMessageForEveryReason()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            await seller.CreateCodeAsync("UNASSIGNED");
            await seller.CreateCodeAsync("DISABLED", isDisabled: true);
            var client = await seller.CreateClientAsync();
            var web = seller.CreateEnforcedSellerWebClient();

            var notAssigned = await MessageAsync(await SaveAsync(web, BasketRequest(client.Id, discountCode: "UNASSIGNED")));
            var unknown = await MessageAsync(await SaveAsync(web, BasketRequest(client.Id, discountCode: "UNKNOWN")));

            // Sellers can fix what is wrong, so they are told what it is (unlike buyers).
            Assert.NotEqual(notAssigned, unknown);
        }

        [Fact]
        public async Task SaveBasket_WithAnUnknownCode_Returns400()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var client = await seller.CreateClientAsync();

            var response = await SaveAsync(seller.CreateEnforcedSellerWebClient(), BasketRequest(client.Id, discountCode: "UNKNOWN"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SaveBasket_WithACodeOfAnotherSeller_Returns400()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var other = await DiscountSeller.CreateAsync(_apiFixture);
            var foreignCodeId = await other.CreateCodeAsync("FOREIGN");
            var foreignClient = await other.CreateClientAsync(new[] { foreignCodeId });
            var client = await seller.CreateClientAsync();

            var forOwnClient = await SaveAsync(seller.CreateEnforcedSellerWebClient(), BasketRequest(client.Id, discountCode: "FOREIGN"));
            var forForeignClient = await SaveAsync(seller.CreateEnforcedSellerWebClient(), BasketRequest(foreignClient.Id, discountCode: "FOREIGN"));

            Assert.Equal(HttpStatusCode.BadRequest, forOwnClient.StatusCode);
            Assert.NotEqual(HttpStatusCode.OK, forForeignClient.StatusCode);
        }

        [Fact]
        public async Task SaveBasket_ForClientAAndThenClientB_RejectsACodeAssignedOnlyToA()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var clientA = await seller.CreateClientAsync(new[] { codeId });
            var clientB = await seller.CreateClientAsync();
            var web = seller.CreateEnforcedSellerWebClient();

            var forA = await SaveOkAsync(web, BasketRequest(clientA.Id, discountCode: Code));
            var forB = await SaveAsync(web, BasketRequest(clientB.Id, discountCode: Code));

            Assert.Equal(Code, forA.DiscountCode);
            Assert.Equal(HttpStatusCode.BadRequest, forB.StatusCode);
        }

        [Fact]
        public async Task SaveBasket_WhenTheStoredCodeIsNoLongerAssigned_RemovesItAndReportsIt()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var web = seller.CreateEnforcedSellerWebClient();
            var saved = await SaveOkAsync(web, BasketRequest(client.Id, discountCode: Code));

            await UnassignAsync(seller, client);

            // The next save does not mention the code, so the stored one is what is checked.
            var next = await SaveOkAsync(web, BasketRequest(client.Id, saved.Id, includeDiscountCode: false));

            Assert.Null(next.DiscountCode);
            Assert.False(string.IsNullOrWhiteSpace(next.DiscountCodeRemovedMessage));
            Assert.Contains(Code, next.DiscountCodeRemovedMessage);

            var stored = await GetStoredBasketAsync(saved.Id.Value);
            Assert.Null(stored.DiscountCode);
            Assert.Null(stored.DiscountCodeClientId);
        }

        [Fact]
        public async Task SaveBasket_WhenTheStoredCodeWasDisabled_RemovesItEvenWhenTheSaveRepeatsItInAnotherSpelling()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var web = seller.CreateEnforcedSellerWebClient();
            var saved = await SaveOkAsync(web, BasketRequest(client.Id, discountCode: Code));

            await seller.UpdateCodeAsync(codeId, null, isDisabled: true);

            var next = await SaveOkAsync(web, BasketRequest(client.Id, saved.Id, discountCode: "summer25"));

            Assert.Null(next.DiscountCode);
            Assert.False(string.IsNullOrWhiteSpace(next.DiscountCodeRemovedMessage));
        }

        [Fact]
        public async Task SaveBasket_WithAnExplicitRemoval_NeedsNoValidation()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var web = seller.CreateEnforcedSellerWebClient();
            var saved = await SaveOkAsync(web, BasketRequest(client.Id, discountCode: Code));

            // Even with the code revoked, a user can always take it off the basket.
            await UnassignAsync(seller, client);
            var removed = await SaveOkAsync(web, BasketRequest(client.Id, saved.Id, discountCode: null));

            Assert.Null(removed.DiscountCode);
            Assert.Null(removed.DiscountCodeRemovedMessage);
            Assert.Null((await GetStoredBasketAsync(saved.Id.Value)).DiscountCode);
        }

        [Fact]
        public async Task SaveBasket_WithEnforcementOff_StillAcceptsAnyCode()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var client = await seller.CreateClientAsync();

            // The default Seller.Web has enforcement off: the legacy behaviour, and the way back if it has to be switched off.
            // It is called as the seller the client belongs to: a seller can only price for its own clients.
            var saved = await SaveOkAsync(_apiFixture.CreateSellerWebClient(seller.Token), BasketRequest(client.Id, discountCode: "ANYTHING"));

            Assert.Equal("ANYTHING", saved.DiscountCode);
            Assert.Null((await GetStoredBasketAsync(saved.Id.Value)).DiscountCodeClientId);
        }

        [Fact]
        public async Task SaveBasket_WhenClientApiCannotBeReached_IsRejectedWithoutChangingTheBasket()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var saved = await SaveOkAsync(seller.CreateEnforcedSellerWebClient(), BasketRequest(client.Id, discountCode: Code));
            var before = await GetStoredBasketAsync(saved.Id.Value);

            var blind = _apiFixture.CreateClientApiUnavailableSellerWebClient(seller.Token);

            // Failing closed: neither the stored code is dropped nor a new one trusted on a transient error.
            var unchanged = await SaveAsync(blind, BasketRequest(client.Id, saved.Id, includeDiscountCode: false));
            var newCode = await SaveAsync(blind, BasketRequest(client.Id, saved.Id, discountCode: "OTHER"));

            Assert.Equal(HttpStatusCode.BadRequest, unchanged.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, newCode.StatusCode);

            var after = await GetStoredBasketAsync(saved.Id.Value);
            Assert.Equal(Code, after.DiscountCode);
            Assert.Equal(before.BasketVersion, after.BasketVersion);
        }

        // ---- Checkout ----

        [Fact]
        public async Task Checkout_WithAValidCodeSavedForTheSameClient_IsAccepted()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var web = seller.CreateEnforcedSellerWebClient();
            var saved = await SaveOkAsync(web, BasketRequest(client.Id, discountCode: Code));

            var response = await CheckoutAsync(web, saved.Id.Value, client);

            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task Checkout_WithoutACode_IsAcceptedWhenTheBasketWasSaved()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var client = await seller.CreateClientAsync();
            var web = seller.CreateEnforcedSellerWebClient();
            var saved = await SaveOkAsync(web, BasketRequest(client.Id, includeDiscountCode: false));

            var response = await CheckoutAsync(web, saved.Id.Value, client);

            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task Checkout_WithACodeThatWasRevokedInTheMeantime_Returns409AndLeavesTheBasketUntouched()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var web = seller.CreateEnforcedSellerWebClient();
            var saved = await SaveOkAsync(web, BasketRequest(client.Id, discountCode: Code));
            var before = await GetStoredBasketAsync(saved.Id.Value);

            await UnassignAsync(seller, client);

            var response = await CheckoutAsync(web, saved.Id.Value, client);

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

            var after = await GetStoredBasketAsync(saved.Id.Value);
            Assert.Equal(Code, after.DiscountCode);
            Assert.Equal(before.BasketVersion, after.BasketVersion);
        }

        [Fact]
        public async Task Checkout_AfterTheRevokedCodeWasRemoved_IsAccepted()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var web = seller.CreateEnforcedSellerWebClient();
            var saved = await SaveOkAsync(web, BasketRequest(client.Id, discountCode: Code));
            await UnassignAsync(seller, client);

            Assert.Equal(HttpStatusCode.Conflict, (await CheckoutAsync(web, saved.Id.Value, client)).StatusCode);

            // Removing the code reprices through the normal write path, which also refreshes the version.
            await SaveOkAsync(web, BasketRequest(client.Id, saved.Id, discountCode: null));

            Assert.Equal(HttpStatusCode.Accepted, (await CheckoutAsync(web, saved.Id.Value, client)).StatusCode);
        }

        [Fact]
        public async Task Checkout_ForAnotherClientThanTheBasketWasPricedFor_Returns409UntilItIsSavedForThatClient()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);

            // The same code is assigned to both, so applicability alone cannot tell the clients apart.
            var clientA = await seller.CreateClientAsync(new[] { codeId });
            var clientB = await seller.CreateClientAsync(new[] { codeId });
            var web = seller.CreateEnforcedSellerWebClient();
            var saved = await SaveOkAsync(web, BasketRequest(clientA.Id, discountCode: Code));

            var forB = await CheckoutAsync(web, saved.Id.Value, clientB);

            Assert.Equal(HttpStatusCode.Conflict, forB.StatusCode);
            Assert.False(string.IsNullOrWhiteSpace(await MessageAsync(forB)));

            // Saving for B (the stored code is re-validated for B) binds the basket to B.
            await SaveOkAsync(web, BasketRequest(clientB.Id, saved.Id, includeDiscountCode: false));

            Assert.Equal(HttpStatusCode.Accepted, (await CheckoutAsync(web, saved.Id.Value, clientB)).StatusCode);
        }

        [Fact]
        public async Task Checkout_WithACodeStoredWithoutTheValidatedClient_Returns409()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });

            // A basket written straight to Basket.Api, as a legacy basket would be: a code and no client metadata.
            var basketId = Guid.NewGuid();
            var legacy = await _apiFixture.BasketApiClient.PostForResponseAsync(
                ApiConstants.Baskets.BasketsApiEndpoint,
                new Dictionary<string, object>
                {
                    ["id"] = basketId,
                    ["discountCode"] = Code,
                    ["items"] = new[]
                    {
                        new Dictionary<string, object>
                        {
                            ["productId"] = Products.Lamica.Id, ["productSku"] = Products.Lamica.Sku, ["productName"] = Products.Lamica.Name, ["quantity"] = 1
                        }
                    }
                });
            Assert.Equal(HttpStatusCode.OK, legacy.StatusCode);

            var response = await CheckoutAsync(seller.CreateEnforcedSellerWebClient(), basketId, client);

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task Checkout_WhenClientApiCannotBeReached_Returns503AndLeavesTheBasketUntouched()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var saved = await SaveOkAsync(seller.CreateEnforcedSellerWebClient(), BasketRequest(client.Id, discountCode: Code));
            var before = await GetStoredBasketAsync(saved.Id.Value);

            var response = await CheckoutAsync(_apiFixture.CreateClientApiUnavailableSellerWebClient(seller.Token), saved.Id.Value, client);

            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);

            var after = await GetStoredBasketAsync(saved.Id.Value);
            Assert.Equal(Code, after.DiscountCode);
            Assert.Equal(before.BasketVersion, after.BasketVersion);
        }

        private static async Task UnassignAsync(DiscountSeller seller, DiscountClient client)
        {
            var response = await seller.SaveClientAsync(new Client.Api.v1.RequestModels.ClientRequestModel
            {
                Id = client.Id,
                Name = client.Email,
                Email = client.Email,
                CommunicationLanguage = "en",
                OrganisationId = client.OrganisationId,
                DiscountCodeIds = Array.Empty<Guid>()
            });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
