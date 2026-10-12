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
using BuyerBasketApiResponseModel = Buyer.Web.Areas.Orders.ApiResponseModels.BasketApiResponseModel;
using SellerBasketResponseModel = Seller.Web.Areas.Orders.ApiResponseModels.BasketResponseModel;

namespace Giuru.IntegrationTests
{
    /// <summary>
    /// The seller app in front of the real Client.Api and Basket.Api. A seller picks the client a basket is for, so the
    /// rules apply to that client, and a seller is told exactly why a code was refused.
    /// </summary>
    [Collection(nameof(ApiCollection))]
    public class SellerDiscountCodeEnforcementTests
    {
        private const string InvalidCodeMessage = "This discount code cannot be applied";
        private const string InactiveMessage = "This discount code is inactive";
        private const string NotAssignedMessage = "This discount code is not assigned to the selected client";
        private const string ClientRequiredMessage = "Select a client before applying a discount code";

        private readonly ApiFixture _apiFixture;

        public SellerDiscountCodeEnforcementTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        private sealed record SellerScenario(DiscountCodeKit Kit, TestClient Client, RestClient Web, DiscountCodeProduct Product);

        private async Task<SellerScenario> CreateSellerAsync()
        {
            var kit = await DiscountCodeKit.CreateAsync(_apiFixture);
            var client = await kit.CreateClientAsync();
            var web = _apiFixture.CreateSellerWebClient(await _apiFixture.IssueSellerTokenAsync(ApiFixture.SellerEmail, ApiFixture.SellerOrganisationId));

            return new SellerScenario(kit, client, web, await _apiFixture.GetDiscountCodeProductAsync());
        }

        private static Dictionary<string, object> Basket(DiscountCodeProduct product, Guid basketId, Guid? clientId, bool withDiscountCode, string discountCode, int quantity = 1, bool withItems = true)
        {
            var body = new Dictionary<string, object>
            {
                ["id"] = basketId,
                ["clientId"] = clientId,
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

        private static async Task<SellerBasketResponseModel> SaveOkAsync(RestClient web, Dictionary<string, object> basket)
        {
            var response = await web.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, basket);

            await RestClient.AssertStatusAsync(response, HttpStatusCode.OK);

            return await RestClient.ReadAsync<SellerBasketResponseModel>(response);
        }

        private async Task<BuyerBasketApiResponseModel> GetPersistedAsync(Guid basketId)
        {
            return await _apiFixture.BasketApiClient.GetAsync<BuyerBasketApiResponseModel>($"{ApiConstants.Baskets.BasketsApiEndpoint}/{basketId}");
        }

        [Fact]
        public async Task Apply_AnAssignedCode_StoresItsCanonicalSpelling()
        {
            var seller = await CreateSellerAsync();
            var basketId = Guid.NewGuid();
            var codeText = DiscountCodeKit.NewCode();
            await seller.Kit.CreateCodeAsync(codeText, new[] { seller.Client.Id });

            var basket = await SaveOkAsync(seller.Web, Basket(seller.Product, basketId, seller.Client.Id, true, $"  {codeText.ToLowerInvariant()}  "));

            Assert.Equal(codeText, basket.DiscountCode);
            Assert.Equal(codeText, (await GetPersistedAsync(basketId)).DiscountCode);
        }

        [Theory]
        [InlineData("unknown", InvalidCodeMessage)]
        [InlineData("assigned-to-another-client", NotAssignedMessage)]
        [InlineData("assigned-to-nobody", NotAssignedMessage)]
        [InlineData("inactive", InactiveMessage)]
        public async Task Apply_ACodeThatCannotBeApplied_IsRefusedWithTheSpecificReasonAndTheBasketIsLeftAlone(string scenario, string expectedMessage)
        {
            var seller = await CreateSellerAsync();
            var otherClient = await seller.Kit.CreateClientAsync();
            var basketId = Guid.NewGuid();
            var codeText = DiscountCodeKit.NewCode();

            switch (scenario)
            {
                case "assigned-to-another-client":
                    await seller.Kit.CreateCodeAsync(codeText, new[] { otherClient.Id });
                    break;
                case "assigned-to-nobody":
                    await seller.Kit.CreateCodeAsync(codeText);
                    break;
                case "inactive":
                    await seller.Kit.CreateCodeAsync(codeText, new[] { seller.Client.Id }, isDisabled: true);
                    break;
            }

            await SaveOkAsync(seller.Web, Basket(seller.Product, basketId, seller.Client.Id, false, null));

            var response = await seller.Web.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, Basket(seller.Product, basketId, seller.Client.Id, true, codeText, quantity: 5));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(expectedMessage, await RestClient.ReadMessageAsync(response));

            var persisted = await GetPersistedAsync(basketId);

            Assert.Null(persisted.DiscountCode);
            Assert.Equal(1, persisted.Items.Single().Quantity);
        }

        [Fact]
        public async Task Apply_ANewCodeWithoutAClient_IsRefusedAskingForOne()
        {
            var seller = await CreateSellerAsync();
            var codeText = DiscountCodeKit.NewCode();
            await seller.Kit.CreateCodeAsync(codeText, new[] { seller.Client.Id });

            var response = await seller.Web.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, Basket(seller.Product, Guid.NewGuid(), null, true, codeText));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(ClientRequiredMessage, await RestClient.ReadMessageAsync(response));
        }

        [Fact]
        public async Task Apply_ForAClientOfAnotherSeller_IsRefused()
        {
            var seller = await CreateSellerAsync();
            var codeText = DiscountCodeKit.NewCode();
            await seller.Kit.CreateCodeAsync(codeText, new[] { seller.Client.Id });
            var otherSellerWeb = _apiFixture.CreateSellerWebClient(await _apiFixture.IssueSellerTokenAsync(ApiFixture.OtherSellerEmail, ApiFixture.OtherSellerOrganisationId));

            var response = await otherSellerWeb.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, Basket(seller.Product, Guid.NewGuid(), seller.Client.Id, true, codeText));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Apply_ANewCodeToAnEmptyBasket_IsRefusedBeforeItIsLookedUp()
        {
            var seller = await CreateSellerAsync();
            var codeText = DiscountCodeKit.NewCode();
            await seller.Kit.CreateCodeAsync(codeText, new[] { seller.Client.Id });

            var response = await seller.Web.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, Basket(seller.Product, Guid.NewGuid(), seller.Client.Id, true, codeText, withItems: false));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var message = await RestClient.ReadMessageAsync(response);

            Assert.NotEqual(InvalidCodeMessage, message);
            Assert.NotEqual(NotAssignedMessage, message);
        }

        [Theory]
        [InlineData("deactivated")]
        [InlineData("unassigned")]
        [InlineData("deleted")]
        [InlineData("renamed")]
        public async Task AStoredCode_SurvivesItsRevocation_ThroughEditsAndResends_EvenWhenTheBasketMovesToAnotherClient(string revocation)
        {
            var seller = await CreateSellerAsync();
            var otherClient = await seller.Kit.CreateClientAsync();
            var basketId = Guid.NewGuid();
            var codeText = DiscountCodeKit.NewCode();
            var code = await seller.Kit.CreateCodeAsync(codeText, new[] { seller.Client.Id });
            await SaveOkAsync(seller.Web, Basket(seller.Product, basketId, seller.Client.Id, true, codeText));

            switch (revocation)
            {
                case "deactivated":
                    await seller.Kit.UpdateSuccessfullyAsync(code, new[] { seller.Client.Id }, isDisabled: true);
                    break;
                case "unassigned":
                    await seller.Kit.UpdateSuccessfullyAsync(code, Array.Empty<Guid>());
                    break;
                case "deleted":
                    Assert.Equal(HttpStatusCode.OK, (await seller.Kit.DeleteAsync(code.Id, code.Version)).StatusCode);
                    break;
                case "renamed":
                    await seller.Kit.UpdateSuccessfullyAsync(code, new[] { seller.Client.Id }, newCode: DiscountCodeKit.NewCode());
                    break;
            }

            var edited = await SaveOkAsync(seller.Web, Basket(seller.Product, basketId, seller.Client.Id, false, null, quantity: 3));
            var resent = await SaveOkAsync(seller.Web, Basket(seller.Product, basketId, seller.Client.Id, true, codeText.ToLowerInvariant(), quantity: 4));

            // A direct request that reuses the basket for another client keeps the stored code too (stored prices are accepted as they are).
            var otherClientEdit = await SaveOkAsync(seller.Web, Basket(seller.Product, basketId, otherClient.Id, true, codeText, quantity: 5));

            Assert.Equal(codeText, edited.DiscountCode);
            Assert.Equal(codeText, resent.DiscountCode);
            Assert.Equal(codeText, otherClientEdit.DiscountCode);

            var persisted = await GetPersistedAsync(basketId);

            Assert.Equal(codeText, persisted.DiscountCode);
            Assert.Equal(5, persisted.Items.Single().Quantity);

            // Revocation only stops new applications.
            var fresh = await seller.Web.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, Basket(seller.Product, Guid.NewGuid(), seller.Client.Id, true, codeText));

            Assert.Equal(HttpStatusCode.BadRequest, fresh.StatusCode);
        }

        [Fact]
        public async Task AStoredCode_CanBeRemovedExplicitly()
        {
            var seller = await CreateSellerAsync();
            var basketId = Guid.NewGuid();
            var codeText = DiscountCodeKit.NewCode();
            var code = await seller.Kit.CreateCodeAsync(codeText, new[] { seller.Client.Id });
            await SaveOkAsync(seller.Web, Basket(seller.Product, basketId, seller.Client.Id, true, codeText));
            await seller.Kit.UpdateSuccessfullyAsync(code, Array.Empty<Guid>());

            var removed = await SaveOkAsync(seller.Web, Basket(seller.Product, basketId, seller.Client.Id, true, null));

            Assert.Null(removed.DiscountCode);
            Assert.Null((await GetPersistedAsync(basketId)).DiscountCode);
        }

        [Fact]
        public async Task AReplacementThatCannotBeApplied_LeavesTheStoredCodeAndItemsAlone()
        {
            var seller = await CreateSellerAsync();
            var basketId = Guid.NewGuid();
            var storedText = DiscountCodeKit.NewCode();
            var notAssigned = DiscountCodeKit.NewCode();
            await seller.Kit.CreateCodeAsync(storedText, new[] { seller.Client.Id });
            await seller.Kit.CreateCodeAsync(notAssigned);
            await SaveOkAsync(seller.Web, Basket(seller.Product, basketId, seller.Client.Id, true, storedText));

            var response = await seller.Web.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, Basket(seller.Product, basketId, seller.Client.Id, true, notAssigned, quantity: 9));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var persisted = await GetPersistedAsync(basketId);

            Assert.Equal(storedText, persisted.DiscountCode);
            Assert.Equal(1, persisted.Items.Single().Quantity);
        }

        private static MultipartFormDataContent CreateUpload(DiscountCodeProduct product, Guid basketId, Guid clientId, bool withDiscountCode, string discountCode)
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.AddWorksheet("Order");

            sheet.Cell(1, 1).Value = "SKU";
            sheet.Cell(1, 2).Value = "Quantity";
            sheet.Cell(2, 1).Value = product.Sku;
            sheet.Cell(2, 2).Value = 2;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            var content = new MultipartFormDataContent
            {
                { new ByteArrayContent(stream.ToArray()), "File", "order.xlsx" },
                { new StringContent(basketId.ToString()), "Id" },
                { new StringContent(clientId.ToString()), "ClientId" }
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
            var seller = await CreateSellerAsync();
            var basketId = Guid.NewGuid();
            var codeText = DiscountCodeKit.NewCode();
            await seller.Kit.CreateCodeAsync(codeText, new[] { seller.Client.Id });

            var response = await seller.Web.PostMultipartForResponseAsync(ApiEndpoints.OrderFileApiEndpoint, CreateUpload(seller.Product, basketId, seller.Client.Id, true, codeText.ToLowerInvariant()));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(codeText, (await RestClient.ReadAsync<SellerBasketResponseModel>(response)).DiscountCode);
        }

        [Fact]
        public async Task Upload_WithACodeThatCannotBeApplied_IsRefusedWithTheSpecificReasonAndStoresNothing()
        {
            var seller = await CreateSellerAsync();
            var basketId = Guid.NewGuid();
            var codeText = DiscountCodeKit.NewCode();
            await seller.Kit.CreateCodeAsync(codeText, new[] { seller.Client.Id }, isDisabled: true);
            await SaveOkAsync(seller.Web, Basket(seller.Product, basketId, seller.Client.Id, false, null));

            var response = await seller.Web.PostMultipartForResponseAsync(ApiEndpoints.OrderFileApiEndpoint, CreateUpload(seller.Product, basketId, seller.Client.Id, true, codeText));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(InactiveMessage, await RestClient.ReadMessageAsync(response));

            var persisted = await GetPersistedAsync(basketId);

            Assert.Null(persisted.DiscountCode);
            Assert.Equal(1, persisted.Items.Single().Quantity);
        }

        [Fact]
        public async Task Upload_ToABasketWhoseStoredCodeWasRevoked_KeepsTheCode()
        {
            var seller = await CreateSellerAsync();
            var basketId = Guid.NewGuid();
            var codeText = DiscountCodeKit.NewCode();
            var code = await seller.Kit.CreateCodeAsync(codeText, new[] { seller.Client.Id });
            await SaveOkAsync(seller.Web, Basket(seller.Product, basketId, seller.Client.Id, true, codeText));
            await seller.Kit.UpdateSuccessfullyAsync(code, Array.Empty<Guid>(), isDisabled: true);

            var response = await seller.Web.PostMultipartForResponseAsync(ApiEndpoints.OrderFileApiEndpoint, CreateUpload(seller.Product, basketId, seller.Client.Id, true, codeText));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(codeText, (await GetPersistedAsync(basketId)).DiscountCode);
        }
    }
}
