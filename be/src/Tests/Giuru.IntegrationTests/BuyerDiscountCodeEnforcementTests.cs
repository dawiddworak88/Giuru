using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using Foundation.ApiExtensions.Shared.Definitions;
using Giuru.IntegrationTests.Definitions;
using Giuru.IntegrationTests.Helpers;
using Giuru.IntegrationTests.HttpClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
    /// <summary>
    /// Buyer.Web with discount code enforcement on. The buyer is identified by the token alone, so every test seeds a
    /// client through Client.Api whose email and organisation match a buyer token, and acts as that buyer.
    /// </summary>
    [Collection(nameof(ApiCollection))]
    public class BuyerDiscountCodeEnforcementTests
    {
        private const string Code = "SUMMER25";

        private readonly ApiFixture _apiFixture;

        public BuyerDiscountCodeEnforcementTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        private static Dictionary<string, object> BasketRequest(string discountCode = null, bool includeDiscountCode = true)
        {
            var request = new Dictionary<string, object>
            {
                ["items"] = new[]
                {
                    new Dictionary<string, object>
                    {
                        ["productId"] = Products.Lamica.Id,
                        ["sku"] = Products.Lamica.Sku,
                        ["name"] = Products.Lamica.Name,
                        ["quantity"] = Inventories.Quantities.Quantity,
                        ["unitPrice"] = 0.01m,
                        ["price"] = 0.01m,
                        ["currency"] = "FAKE"
                    }
                }
            };

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

        private static async Task<BasketResponseModel> SaveOkAsync(RestClient web, Dictionary<string, object> request)
        {
            var response = await SaveAsync(web, request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            return await RestClient.ReadAsync<BasketResponseModel>(response);
        }

        private Task<BasketApiResponseModel> GetStoredBasketAsync(Guid basketId)
        {
            return _apiFixture.BasketApiClient.GetAsync<BasketApiResponseModel>($"{ApiConstants.Baskets.BasketsApiEndpoint}/{basketId}");
        }

        private static Task<HttpResponseMessage> CheckoutAsync(RestClient web, Guid? basketId = null, Guid? clientId = null)
        {
            return web.PostForResponseAsync(
                ApiEndpoints.OrderCheckoutApiEndpoint,
                new CheckoutBasketRequestModel { BasketId = basketId, ClientId = clientId, ClientName = "Buyer" });
        }

        private static async Task<string> MessageAsync(HttpResponseMessage response)
        {
            return (await RestClient.ReadAsync<MessageResponse>(response))?.Message;
        }

        private sealed class MessageResponse
        {
            public string Message { get; set; }
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

        // ---- Writing a basket ----

        [Fact]
        public async Task SaveBasket_WithAnAssignedCode_StoresTheCanonicalSpellingAndTheBuyersClient_WhileUnreachableGrulaClearsThePrices()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var web = await client.CreateEnforcedBuyerWebClientAsync();

            var saved = await SaveOkAsync(web, BasketRequest("summer25"));

            // The code is kept - verification and pricing are separate things - but Grula cannot be reached, so the
            // basket carries no trusted prices (the same as without a code, see BasketPricingTests).
            Assert.Equal(Code, saved.DiscountCode);
            Assert.Null(saved.DiscountCodeRemovedMessage);
            var line = saved.Items.Single();
            Assert.Null(line.UnitPrice);
            Assert.Null(line.Price);
            Assert.Null(line.Currency);

            var stored = await GetStoredBasketAsync(saved.Id.Value);
            Assert.Equal(Code, stored.DiscountCode);
            Assert.Equal(client.Id, stored.DiscountCodeClientId);
            Assert.NotNull(stored.BasketVersion);
        }

        [Fact]
        public async Task SaveBasket_WithAnUnregisteredCode_IsRejected()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var client = await seller.CreateClientAsync();
            var web = await client.CreateEnforcedBuyerWebClientAsync();

            var response = await SaveAsync(web, BasketRequest(Code));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SaveBasket_WithACodeThatCannotBeApplied_GivesTheBuyerOneMessageWhateverTheReason()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var other = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync("ASSIGNED");
            await seller.CreateCodeAsync("DISABLED", isDisabled: true);
            await seller.CreateCodeAsync("UNASSIGNED");
            await other.CreateCodeAsync("FOREIGN");
            var client = await seller.CreateClientAsync(new[] { codeId });
            var web = await client.CreateEnforcedBuyerWebClientAsync();

            var messages = new List<string>();

            foreach (var code in new[] { "UNKNOWN", "DISABLED", "UNASSIGNED", "FOREIGN" })
            {
                var response = await SaveAsync(web, BasketRequest(code));

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                messages.Add(await MessageAsync(response));
            }

            // A buyer must not be able to probe which codes exist or who they are assigned to.
            Assert.False(string.IsNullOrWhiteSpace(messages[0]));
            Assert.Single(messages.Distinct());
        }

        [Fact]
        public async Task SaveBasket_WithARejectedCode_LeavesTheStoredBasketUntouched()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var client = await seller.CreateClientAsync();
            var web = await client.CreateEnforcedBuyerWebClientAsync();
            var existing = await SaveOkAsync(web, BasketRequest(includeDiscountCode: false));
            var before = await GetStoredBasketAsync(existing.Id.Value);

            var response = await SaveAsync(web, BasketRequest(Code));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var after = await GetStoredBasketAsync(existing.Id.Value);
            Assert.Null(after.DiscountCode);
            Assert.Equal(before.BasketVersion, after.BasketVersion);
        }

        [Fact]
        public async Task SaveBasket_ForABuyerWithoutAClient_CannotApplyAnyCode()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            await seller.CreateCodeAsync(Code);
            var token = await _apiFixture.GetTokenAsync($"nobody-{Guid.NewGuid():N}@tests.com", "none", Guid.NewGuid());

            var response = await SaveAsync(_apiFixture.CreateEnforcedBuyerWebClient(token), BasketRequest(Code));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SaveBasket_WhenTheStoredCodeIsNoLongerAssigned_RemovesItAndReportsIt()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var web = await client.CreateEnforcedBuyerWebClientAsync();
            var saved = await SaveOkAsync(web, BasketRequest(Code));

            await UnassignAsync(seller, client);

            var next = await SaveOkAsync(web, BasketRequest(includeDiscountCode: false));

            Assert.Null(next.DiscountCode);
            Assert.False(string.IsNullOrWhiteSpace(next.DiscountCodeRemovedMessage));

            var stored = await GetStoredBasketAsync(saved.Id.Value);
            Assert.Null(stored.DiscountCode);
            Assert.Null(stored.DiscountCodeClientId);
        }

        [Fact]
        public async Task SaveBasket_WithEnforcementOff_StillStoresAnyCode()
        {
            // The legacy behaviour, which is also the way back if enforcement has to be switched off: the original
            // BasketDiscountCodeTests case, now explicitly against the Buyer.Web without enforcement.
            var saved = await SaveOkAsync(_apiFixture.BuyerWebClient, BasketRequest("ANYTHING"));

            Assert.Equal("ANYTHING", saved.DiscountCode);
        }

        // ---- Checkout ----

        [Fact]
        public async Task Checkout_WithAValidCode_IsAccepted()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var web = await client.CreateEnforcedBuyerWebClientAsync();
            var saved = await SaveOkAsync(web, BasketRequest(Code));

            var response = await CheckoutAsync(web, saved.Id);

            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task Checkout_WithACodeThatWasRevokedInTheMeantime_Returns409()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var web = await client.CreateEnforcedBuyerWebClientAsync();
            var saved = await SaveOkAsync(web, BasketRequest(Code));

            await UnassignAsync(seller, client);

            var response = await CheckoutAsync(web, saved.Id);

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            Assert.False(string.IsNullOrWhiteSpace(await MessageAsync(response)));
            Assert.Equal(Code, (await GetStoredBasketAsync(saved.Id.Value)).DiscountCode);
        }

        [Fact]
        public async Task Checkout_CannotBeSteeredToAnotherBasketThroughTheBody()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var web = await client.CreateEnforcedBuyerWebClientAsync();
            await SaveOkAsync(web, BasketRequest(Code));
            await UnassignAsync(seller, client);

            // The checks and the order use the basket of the cookie, so naming some other basket changes nothing.
            var response = await CheckoutAsync(web, basketId: Guid.NewGuid());

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task Checkout_CannotBeSteeredToAnotherClientThroughTheBody()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var neighbour = await seller.CreateClientAsync(new[] { codeId });
            var web = await client.CreateEnforcedBuyerWebClientAsync();
            var saved = await SaveOkAsync(web, BasketRequest(Code));

            var response = await CheckoutAsync(web, saved.Id, clientId: neighbour.Id);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(Code, (await GetStoredBasketAsync(saved.Id.Value)).DiscountCode);
        }

        [Fact]
        public async Task Checkout_WithABodyClientThatMatchesThePrincipal_IsAccepted()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var web = await client.CreateEnforcedBuyerWebClientAsync();
            var saved = await SaveOkAsync(web, BasketRequest(Code));

            var response = await CheckoutAsync(web, saved.Id, clientId: client.Id);

            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task Checkout_WithACodeStoredWithoutTheValidatedClient_Returns409()
        {
            var seller = await DiscountSeller.CreateAsync(_apiFixture);
            var codeId = await seller.CreateCodeAsync(Code);
            var client = await seller.CreateClientAsync(new[] { codeId });
            var web = await client.CreateEnforcedBuyerWebClientAsync();

            // The cookie basket is created by the first save; replace it behind the web tier's back with a legacy-style
            // basket: a code and no client metadata.
            var saved = await SaveOkAsync(web, BasketRequest(includeDiscountCode: false));
            var legacy = await _apiFixture.BasketApiClient.PostForResponseAsync(
                ApiConstants.Baskets.BasketsApiEndpoint,
                new Dictionary<string, object>
                {
                    ["id"] = saved.Id.Value,
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

            var response = await CheckoutAsync(web, saved.Id);

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }
    }
}
