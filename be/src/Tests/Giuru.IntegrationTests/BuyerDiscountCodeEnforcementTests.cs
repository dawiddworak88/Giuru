using Buyer.Web.Areas.Orders.ApiResponseModels;
using ClosedXML.Excel;
using Foundation.ApiExtensions.Shared.Definitions;
using Giuru.IntegrationTests.Definitions;
using Giuru.IntegrationTests.Helpers;
using Giuru.IntegrationTests.HttpClients;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
    /// <summary>
    /// The buyer app in front of the real Client.Api and Basket.Api. Grula is configured but unreachable, so what is
    /// asserted is the application rule - which codes may price a basket, newly applied or already stored - never a price.
    /// </summary>
    [Collection(nameof(ApiCollection))]
    public class BuyerDiscountCodeEnforcementTests
    {
        private const string InvalidCodeMessage = "This discount code cannot be applied";

        private static string RemovedCodeMessage(string code) => $"The discount code {code} is no longer valid and has been removed. The prices have been updated.";

        private static string NoLongerValidAtCheckoutMessage(string code) => $"The discount code {code} is no longer valid. Remove it to update the prices, then place the order again.";

        private readonly ApiFixture _apiFixture;

        public BuyerDiscountCodeEnforcementTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        private sealed record BuyerScenario(DiscountCodeKit Kit, TestClient Client, RestClient Web, DiscountCodeProduct Product);

        private async Task<BuyerScenario> CreateBuyerAsync()
        {
            var kit = await DiscountCodeKit.CreateAsync(_apiFixture);
            var client = await kit.CreateClientAsync();

            return new BuyerScenario(kit, client, await kit.CreateBuyerWebClientAsync(client), await _apiFixture.GetDiscountCodeProductAsync());
        }

        private static Dictionary<string, object> Basket(DiscountCodeProduct product, bool withDiscountCode, string discountCode, int quantity = 1, bool withItems = true)
        {
            var body = new Dictionary<string, object>
            {
                ["items"] = withItems
                    ? new object[] { new { productId = product.Id, sku = product.Sku, name = product.Name, quantity } }
                    : Array.Empty<object>()
            };

            if (withDiscountCode)
            {
                // A null code is an explicit removal; leaving the property out means "keep what is stored".
                body["discountCode"] = discountCode;
            }

            return body;
        }

        private static async Task<BasketResponseModel> SaveOkAsync(RestClient web, Dictionary<string, object> basket)
        {
            var response = await web.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, basket);

            await RestClient.AssertStatusAsync(response, HttpStatusCode.OK);

            return await RestClient.ReadAsync<BasketResponseModel>(response);
        }

        /// <summary>A save that dropped the stored code: the basket is saved, repriced without it, and the 422 carries it.</summary>
        private static async Task<BasketResponseModel> AssertSavedWithoutTheCodeAsync(HttpResponseMessage response, string removedCode)
        {
            await RestClient.AssertStatusAsync(response, HttpStatusCode.UnprocessableEntity);

            var basket = await RestClient.ReadAsync<BasketResponseModel>(response);

            Assert.Equal(RemovedCodeMessage(removedCode), basket.Message);
            Assert.NotNull(basket.Id);
            Assert.Null(basket.DiscountCode);

            return basket;
        }

        private async Task<BasketApiResponseModel> GetPersistedAsync(Guid? basketId)
        {
            return await _apiFixture.BasketApiClient.GetAsync<BasketApiResponseModel>($"{ApiConstants.Baskets.BasketsApiEndpoint}/{basketId}");
        }

        [Fact]
        public async Task Apply_AnAssignedCode_StoresItsCanonicalSpellingAndPricesWithIt()
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });

            var basket = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, $"  {codeText.ToLowerInvariant()}  "));

            Assert.Equal(codeText, basket.DiscountCode);
            Assert.Equal(codeText, (await GetPersistedAsync(basket.Id)).DiscountCode);
        }

        [Fact]
        public async Task ACopiedBasketCookie_DoesNotLetAnotherClientUseOrCheckoutTheDiscount()
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });
            var applied = await buyer.Web.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, Basket(buyer.Product, true, codeText));
            await RestClient.AssertStatusAsync(applied, HttpStatusCode.OK);
            var basket = await RestClient.ReadAsync<BasketResponseModel>(applied);
            var copiedCookies = string.Join("; ", applied.Headers.GetValues("Set-Cookie").Select(x => x.Split(';')[0]));

            // The basket cookie also survives a logout, so this is what the next account in the same browser sends.
            var otherClient = await buyer.Kit.CreateClientAsync();
            var otherBuyer = await buyer.Kit.CreateBuyerWebClientAsync(otherClient);
            var checkout = await otherBuyer.PostForResponseAsync(ApiEndpoints.OrderCheckoutApiEndpoint,
                new { BasketId = basket.Id, ClientId = buyer.Client.Id }, copiedCookies);

            await RestClient.AssertStatusAsync(checkout, HttpStatusCode.BadRequest);
            Assert.Equal(NoLongerValidAtCheckoutMessage(codeText), await RestClient.ReadMessageAsync(checkout));

            // Their edit is not lost: it is saved, priced without the code, and the 422 tells them why the prices changed.
            var save = await otherBuyer.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint,
                Basket(buyer.Product, false, null, quantity: 5), copiedCookies);

            await AssertSavedWithoutTheCodeAsync(save, codeText);

            var persisted = await GetPersistedAsync(basket.Id);

            Assert.Null(persisted.DiscountCode);
            Assert.Equal(5, persisted.Items.Single().Quantity);
        }

        [Fact]
        public async Task Apply_ByAMemberOfTheClientsTeam_IsAcceptedOnTheirVeryFirstRequest()
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });
            var teamMember = await buyer.Kit.CreateTeamMemberWebClientAsync(buyer.Client);

            // Nothing about this identity has been seen before: no cached claims, no client record under its email.
            var basket = await SaveOkAsync(teamMember, Basket(buyer.Product, true, codeText));

            Assert.Equal(codeText, basket.DiscountCode);
        }

        [Fact]
        public async Task Apply_ByABuyerOfAnotherClient_IsRefusedWithTheGenericMessage()
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });
            var other = await buyer.Kit.CreateClientAsync();
            var otherWeb = await buyer.Kit.CreateBuyerWebClientAsync(other);

            var response = await otherWeb.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, Basket(buyer.Product, true, codeText));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(InvalidCodeMessage, await RestClient.ReadMessageAsync(response));
        }

        [Theory]
        [InlineData("unknown")]
        [InlineData("assigned-to-another-client")]
        [InlineData("assigned-to-nobody")]
        [InlineData("inactive")]
        public async Task Apply_ACodeThatCannotBeApplied_IsRefusedWithOneGenericMessageAndTheBasketIsLeftAlone(string scenario)
        {
            var buyer = await CreateBuyerAsync();
            var otherClient = await buyer.Kit.CreateClientAsync();
            var codeText = DiscountCodeKit.NewCode();

            switch (scenario)
            {
                case "assigned-to-another-client":
                    await buyer.Kit.CreateCodeAsync(codeText, new[] { otherClient.Id });
                    break;
                case "assigned-to-nobody":
                    await buyer.Kit.CreateCodeAsync(codeText);
                    break;
                case "inactive":
                    await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id }, isDisabled: true);
                    break;
            }

            var existing = await SaveOkAsync(buyer.Web, Basket(buyer.Product, false, null));

            var response = await buyer.Web.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, Basket(buyer.Product, true, codeText, quantity: 5));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(InvalidCodeMessage, await RestClient.ReadMessageAsync(response));

            var persisted = await GetPersistedAsync(existing.Id);

            Assert.Null(persisted.DiscountCode);
            Assert.Equal(1, persisted.Items.Single().Quantity);
        }

        [Fact]
        public async Task Apply_ANewCodeToAnEmptyBasket_IsRefusedBeforeItIsLookedUp()
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });

            var response = await buyer.Web.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, Basket(buyer.Product, true, codeText, withItems: false));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEqual(InvalidCodeMessage, await RestClient.ReadMessageAsync(response));
        }

        public static IEnumerable<object[]> Revocations => new[]
        {
            new object[] { "deactivated" },
            new object[] { "unassigned" },
            new object[] { "deleted" },
            new object[] { "renamed" }
        };

        private static async Task RevokeAsync(DiscountCodeKit kit, string revocation, SavedDiscountCode code)
        {
            switch (revocation)
            {
                case "deactivated":
                    await kit.UpdateSuccessfullyAsync(code, await AssignedClientIdsAsync(kit, code), isDisabled: true);
                    break;
                case "unassigned":
                    await kit.UpdateSuccessfullyAsync(code, Array.Empty<Guid>());
                    break;
                case "deleted":
                    Assert.Equal(HttpStatusCode.OK, (await kit.DeleteAsync(code.Id, code.Version)).StatusCode);
                    break;
                case "renamed":
                    await kit.UpdateSuccessfullyAsync(code, await AssignedClientIdsAsync(kit, code), newCode: DiscountCodeKit.NewCode());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(revocation));
            }
        }

        private static async Task<IEnumerable<Guid>> AssignedClientIdsAsync(DiscountCodeKit kit, SavedDiscountCode code)
        {
            return (await kit.GetAsync(code.Id)).ClientIds;
        }

        [Theory]
        [MemberData(nameof(Revocations))]
        public async Task AStoredCode_ThatWasRevoked_IsRemovedByTheNextSave_WhichStillGoesAhead(string revocation)
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            var code = await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });
            var basket = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, codeText));
            Assert.Equal(codeText, basket.DiscountCode);

            await RevokeAsync(buyer.Kit, revocation, code);

            // The stored code is validated again by every save that prices it.
            var edited = await buyer.Web.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, Basket(buyer.Product, false, null, quantity: 3));

            var saved = await AssertSavedWithoutTheCodeAsync(edited, codeText);
            Assert.Equal(basket.Id, saved.Id);

            var persisted = await GetPersistedAsync(basket.Id);

            Assert.Null(persisted.DiscountCode);
            Assert.Equal(3, persisted.Items.Single().Quantity);

            // Once removed, sending the text again is applying a new code, which is refused and leaves the basket alone.
            var resent = await buyer.Web.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, Basket(buyer.Product, true, codeText.ToLowerInvariant(), quantity: 4));

            Assert.Equal(HttpStatusCode.BadRequest, resent.StatusCode);
            Assert.Equal(InvalidCodeMessage, await RestClient.ReadMessageAsync(resent));
            Assert.Equal(3, (await GetPersistedAsync(basket.Id)).Items.Single().Quantity);
        }

        [Fact]
        public async Task AStoredCode_ThatWasRevoked_IsRemovedEvenWhenTheFrontendSendsItAgain()
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            var code = await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });
            var basket = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, codeText));
            await buyer.Kit.UpdateSuccessfullyAsync(code, Array.Empty<Guid>());

            var resent = await buyer.Web.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, Basket(buyer.Product, true, codeText.ToLowerInvariant(), quantity: 4));

            await AssertSavedWithoutTheCodeAsync(resent, codeText);

            var persisted = await GetPersistedAsync(basket.Id);

            Assert.Null(persisted.DiscountCode);
            Assert.Equal(4, persisted.Items.Single().Quantity);
        }

        [Fact]
        public async Task AStoredCode_CanBeRemovedExplicitly_AndAThenRevokedOneCannotBeAppliedBack()
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            var code = await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });
            var basket = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, codeText));
            await buyer.Kit.UpdateSuccessfullyAsync(code, Array.Empty<Guid>());

            var removed = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, null));

            Assert.Null(removed.DiscountCode);
            Assert.Null((await GetPersistedAsync(basket.Id)).DiscountCode);

            var again = await buyer.Web.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, Basket(buyer.Product, true, codeText));

            Assert.Equal(HttpStatusCode.BadRequest, again.StatusCode);
        }

        [Fact]
        public async Task AReplacementThatCannotBeApplied_LeavesTheStoredCodeAndItemsAlone()
        {
            var buyer = await CreateBuyerAsync();
            var storedText = DiscountCodeKit.NewCode();
            await buyer.Kit.CreateCodeAsync(storedText, new[] { buyer.Client.Id });
            var notAssigned = DiscountCodeKit.NewCode();
            await buyer.Kit.CreateCodeAsync(notAssigned);
            var basket = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, storedText));

            var response = await buyer.Web.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, Basket(buyer.Product, true, notAssigned, quantity: 9));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var persisted = await GetPersistedAsync(basket.Id);

            Assert.Equal(storedText, persisted.DiscountCode);
            Assert.Equal(1, persisted.Items.Single().Quantity);
        }

        [Fact]
        public async Task AReplacementThatCanBeApplied_TakesTheStoredCodesPlace()
        {
            var buyer = await CreateBuyerAsync();
            var storedText = DiscountCodeKit.NewCode();
            var replacementText = DiscountCodeKit.NewCode();
            await buyer.Kit.CreateCodeAsync(storedText, new[] { buyer.Client.Id });
            await buyer.Kit.CreateCodeAsync(replacementText, new[] { buyer.Client.Id });
            var basket = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, storedText));

            var replaced = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, replacementText));

            Assert.Equal(replacementText, replaced.DiscountCode);
            Assert.Equal(replacementText, (await GetPersistedAsync(basket.Id)).DiscountCode);
        }

        [Fact]
        public async Task ACaseOnlyRename_GivesTheStoredBasketTheCurrentSpellingAtItsNextSave()
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            var code = await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });
            var basket = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, codeText));

            await buyer.Kit.UpdateSuccessfullyAsync(code, new[] { buyer.Client.Id }, newCode: codeText.ToLowerInvariant());

            var edited = await SaveOkAsync(buyer.Web, Basket(buyer.Product, false, null, quantity: 2));

            Assert.Equal(codeText.ToLowerInvariant(), edited.DiscountCode);
            Assert.Equal(codeText.ToLowerInvariant(), (await GetPersistedAsync(basket.Id)).DiscountCode);
        }

        [Fact]
        public async Task ARecreatedCode_PricesABasketThatHoldsItsText_OnlyAccordingToItsOwnAssignments()
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            var original = await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });
            var basket = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, codeText));
            Assert.Equal(HttpStatusCode.OK, (await buyer.Kit.DeleteAsync(original.Id, original.Version)).StatusCode);

            // The basket holds only the text, so it is the recreated code, with its own assignments, that is validated.
            var recreated = await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });

            var accepted = await SaveOkAsync(buyer.Web, Basket(buyer.Product, false, null, quantity: 2));
            Assert.Equal(codeText, accepted.DiscountCode);

            await buyer.Kit.UpdateSuccessfullyAsync(recreated, Array.Empty<Guid>());

            var removed = await buyer.Web.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, Basket(buyer.Product, false, null, quantity: 3));
            await AssertSavedWithoutTheCodeAsync(removed, codeText);

            var persisted = await GetPersistedAsync(basket.Id);

            Assert.Null(persisted.DiscountCode);
            Assert.Equal(3, persisted.Items.Single().Quantity);
        }

        [Fact]
        public async Task Checkout_OfABasketWhoseStoredCodeWasRevoked_IsRefusedUntilTheCodeIsRemoved()
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            var code = await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });
            var basket = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, codeText));
            await buyer.Kit.UpdateSuccessfullyAsync(code, Array.Empty<Guid>(), isDisabled: true);
            var checkoutRequest = new
            {
                basketId = basket.Id,
                clientId = buyer.Client.Id,
                clientName = buyer.Client.Name
            };

            var refused = await buyer.Web.PostForResponseAsync(ApiEndpoints.OrderCheckoutApiEndpoint, checkoutRequest);

            await RestClient.AssertStatusAsync(refused, HttpStatusCode.BadRequest);
            Assert.Equal(NoLongerValidAtCheckoutMessage(codeText), await RestClient.ReadMessageAsync(refused));
            Assert.Equal(codeText, (await GetPersistedAsync(basket.Id)).DiscountCode);

            // Removing the code reprices the basket without it, and that basket can be ordered.
            await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, null));

            var accepted = await buyer.Web.PostForResponseAsync(ApiEndpoints.OrderCheckoutApiEndpoint, checkoutRequest);

            await RestClient.AssertStatusAsync(accepted, HttpStatusCode.Accepted);
        }

        private static byte[] CreateOrderFile(DiscountCodeProduct product, int quantity)
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.AddWorksheet("Order");

            sheet.Cell(1, 1).Value = "SKU";
            sheet.Cell(1, 2).Value = "Quantity";
            sheet.Cell(2, 1).Value = product.Sku;
            sheet.Cell(2, 2).Value = quantity;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }

        private static MultipartFormDataContent CreateUpload(DiscountCodeProduct product, int quantity, bool withDiscountCode, string discountCode)
        {
            var content = new MultipartFormDataContent
            {
                { new ByteArrayContent(CreateOrderFile(product, quantity)), "File", "order.xlsx" }
            };

            if (withDiscountCode)
            {
                content.Add(new StringContent(discountCode ?? string.Empty), "DiscountCode");
            }

            return content;
        }

        [Fact]
        public async Task Upload_WithANewAssignedCode_StoresItsCanonicalSpelling()
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });

            var response = await buyer.Web.PostMultipartForResponseAsync(ApiEndpoints.OrderFileApiEndpoint, CreateUpload(buyer.Product, 2, true, $" {codeText.ToLowerInvariant()} "));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(codeText, (await RestClient.ReadAsync<BasketResponseModel>(response)).DiscountCode);
        }

        [Fact]
        public async Task Upload_WithACodeThatCannotBeApplied_IsRefusedAndStoresNothing()
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            await buyer.Kit.CreateCodeAsync(codeText);
            var existing = await SaveOkAsync(buyer.Web, Basket(buyer.Product, false, null));

            var response = await buyer.Web.PostMultipartForResponseAsync(ApiEndpoints.OrderFileApiEndpoint, CreateUpload(buyer.Product, 2, true, codeText));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(InvalidCodeMessage, await RestClient.ReadMessageAsync(response));

            var persisted = await GetPersistedAsync(existing.Id);

            Assert.Null(persisted.DiscountCode);
            Assert.Equal(1, persisted.Items.Single().Quantity);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Upload_ToABasketWhoseStoredCodeWasRevoked_IsSavedWithoutTheCode(bool resendTheCode)
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            var code = await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });
            var basket = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, codeText));
            await buyer.Kit.UpdateSuccessfullyAsync(code, Array.Empty<Guid>(), isDisabled: true);

            var response = await buyer.Web.PostMultipartForResponseAsync(ApiEndpoints.OrderFileApiEndpoint, CreateUpload(buyer.Product, 2, resendTheCode, codeText));

            await AssertSavedWithoutTheCodeAsync(response, codeText);

            var persisted = await GetPersistedAsync(basket.Id);
            var line = persisted.Items.Single();

            Assert.Null(persisted.DiscountCode);
            Assert.Equal(3, line.Quantity + line.StockQuantity);
        }
    }
}
