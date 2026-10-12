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
    /// asserted is the application rule - which codes may be newly applied and which stored ones are kept - never a price.
    /// </summary>
    [Collection(nameof(ApiCollection))]
    public class BuyerDiscountCodeEnforcementTests
    {
        private const string InvalidCodeMessage = "This discount code cannot be applied";

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
        public async Task AStoredCode_SurvivesItsRevocation_ThroughEditsAndResends_ButCannotBeAppliedAgain(string revocation)
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            var code = await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });
            var basket = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, codeText));
            Assert.Equal(codeText, basket.DiscountCode);

            await RevokeAsync(buyer.Kit, revocation, code);

            // An edit that leaves the code out keeps it.
            var edited = await SaveOkAsync(buyer.Web, Basket(buyer.Product, false, null, quantity: 3));
            Assert.Equal(codeText, edited.DiscountCode);

            // The frontend sends the applied code with every edit; the same text, in any casing, is the stored code.
            var resent = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, codeText, quantity: 4));
            var resentInOtherCasing = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, codeText.ToLowerInvariant(), quantity: 5));

            Assert.Equal(codeText, resent.DiscountCode);
            Assert.Equal(codeText, resentInOtherCasing.DiscountCode);

            var persisted = await GetPersistedAsync(basket.Id);

            Assert.Equal(codeText, persisted.DiscountCode);
            Assert.Equal(5, persisted.Items.Single().Quantity);

            // Revocation only stops new applications.
            var freshWeb = await buyer.Kit.CreateBuyerWebClientAsync(buyer.Client);
            var fresh = await freshWeb.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, Basket(buyer.Product, true, codeText));

            Assert.Equal(HttpStatusCode.BadRequest, fresh.StatusCode);
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
        public async Task ACaseOnlyRename_LeavesTheStoredBasketWithItsOriginalSpelling()
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            var code = await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });
            await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, codeText));

            await buyer.Kit.UpdateSuccessfullyAsync(code, new[] { buyer.Client.Id }, newCode: codeText.ToLowerInvariant());

            var resent = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, codeText.ToLowerInvariant(), quantity: 2));

            Assert.Equal(codeText, resent.DiscountCode);

            // A basket that starts now gets the current spelling.
            var freshWeb = await buyer.Kit.CreateBuyerWebClientAsync(buyer.Client);
            var fresh = await SaveOkAsync(freshWeb, Basket(buyer.Product, true, codeText));

            Assert.Equal(codeText.ToLowerInvariant(), fresh.DiscountCode);
        }

        [Fact]
        public async Task ARecreatedCode_DoesNotChangeABasketThatAlreadyHoldsTheText_AndIsAppliedAccordingToItsOwnAssignments()
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            var original = await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });
            var basket = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, codeText));
            Assert.Equal(HttpStatusCode.OK, (await buyer.Kit.DeleteAsync(original.Id, original.Version)).StatusCode);

            var recreated = await buyer.Kit.CreateCodeAsync(codeText);

            var kept = await SaveOkAsync(buyer.Web, Basket(buyer.Product, false, null, quantity: 2));
            Assert.Equal(codeText, kept.DiscountCode);

            var freshWeb = await buyer.Kit.CreateBuyerWebClientAsync(buyer.Client);
            var refused = await freshWeb.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, Basket(buyer.Product, true, codeText));
            Assert.Equal(HttpStatusCode.BadRequest, refused.StatusCode);

            await buyer.Kit.UpdateSuccessfullyAsync(recreated, new[] { buyer.Client.Id });

            var secondFreshWeb = await buyer.Kit.CreateBuyerWebClientAsync(buyer.Client);
            var accepted = await SaveOkAsync(secondFreshWeb, Basket(buyer.Product, true, codeText));
            Assert.Equal(codeText, accepted.DiscountCode);
            Assert.Equal(codeText, (await GetPersistedAsync(basket.Id)).DiscountCode);
        }

        [Fact]
        public async Task Checkout_OfABasketWhoseStoredCodeWasRevoked_Proceeds()
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            var code = await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });
            var basket = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, codeText));
            await buyer.Kit.UpdateSuccessfullyAsync(code, Array.Empty<Guid>(), isDisabled: true);

            var checkout = await buyer.Web.PostForResponseAsync(ApiEndpoints.OrderCheckoutApiEndpoint, new
            {
                basketId = basket.Id,
                clientId = buyer.Client.Id,
                clientName = buyer.Client.Name
            });

            Assert.Equal(HttpStatusCode.Accepted, checkout.StatusCode);
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
        public async Task Upload_ToABasketWhoseStoredCodeWasRevoked_KeepsTheCode(bool resendTheCode)
        {
            var buyer = await CreateBuyerAsync();
            var codeText = DiscountCodeKit.NewCode();
            var code = await buyer.Kit.CreateCodeAsync(codeText, new[] { buyer.Client.Id });
            var basket = await SaveOkAsync(buyer.Web, Basket(buyer.Product, true, codeText));
            await buyer.Kit.UpdateSuccessfullyAsync(code, Array.Empty<Guid>(), isDisabled: true);

            var response = await buyer.Web.PostMultipartForResponseAsync(ApiEndpoints.OrderFileApiEndpoint, CreateUpload(buyer.Product, 2, resendTheCode, codeText));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(codeText, (await RestClient.ReadAsync<BasketResponseModel>(response)).DiscountCode);
            Assert.Equal(codeText, (await GetPersistedAsync(basket.Id)).DiscountCode);
        }
    }
}
