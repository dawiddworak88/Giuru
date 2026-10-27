using Catalog.Api.v1.Products.RequestModels;
using ClosedXML.Excel;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Shared.Definitions;
using Giuru.IntegrationTests.Definitions;
using Giuru.IntegrationTests.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BuyerBasketItemRequestModel = Buyer.Web.Areas.Orders.ApiRequestModels.BasketItemRequestModel;
using BuyerBasketResponseModel = Buyer.Web.Areas.Orders.ApiResponseModels.BasketResponseModel;
using BuyerSaveBasketRequestModel = Buyer.Web.Areas.Orders.ApiRequestModels.SaveBasketRequestModel;
using ClientRequestModel = Client.Api.v1.RequestModels.ClientRequestModel;
using ClientResponseModel = Client.Api.v1.ResponseModels.ClientResponseModel;
using DiscountCodeRequestModel = Seller.Web.Areas.Clients.ApiRequestModels.DiscountCodeRequestModel;
using SellerBasketApiResponseModel = Seller.Web.Areas.Orders.ApiResponseModels.BasketApiResponseModel;
using SellerBasketItemRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.BasketItemRequestModel;
using SellerBasketResponseModel = Seller.Web.Areas.Orders.ApiResponseModels.BasketResponseModel;
using SellerSaveBasketRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.SaveBasketRequestModel;

namespace Giuru.IntegrationTests
{
    // A Discount Code is accepted on a Basket save only when it is a Valid Discount Code for the Client
    // concerned, and a stored code that stops being valid is dropped with a message. Basket lines here
    // use a SKU no catalog product has, so a save runs the real eligibility check and price-client
    // lookup but has nothing to price - Grula is unreachable in this fixture anyway. The order-file
    // upload refuses a file without a known product, so its tests create one.
    [Collection(nameof(ApiCollection))]
    public class DiscountCodeEnforcementTests
    {
        private const string ClientsEndpoint = "api/v1/Clients";
        private const string TestUserEmail = "seller@user.com";

        private readonly ApiFixture _apiFixture;

        public DiscountCodeEnforcementTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        [Fact]
        public async Task SellerSaveBasket_WithAValidCodeTypedInAnotherCase_StoresItsStoredSpelling()
        {
            var clientId = await CreateClientAsync();

            try
            {
                var code = NewCode();
                await CreateCodeAsync(code, clientId);
                var basketId = Guid.NewGuid();

                var saved = await SaveSellerBasketAsync(basketId, clientId, code.ToLowerInvariant());

                Assert.Equal(code, saved.DiscountCode);
                Assert.Null(saved.DiscountCodeRemovedMessage);
                Assert.Equal(code, (await GetPersistedBasketAsync(basketId)).DiscountCode);
            }
            finally
            {
                await DeleteClientAsync(clientId);
            }
        }

        [Fact]
        public async Task SellerSaveBasket_WithACodeNotValidForTheClient_IsRejectedAndLeavesTheBasketUnchanged()
        {
            var clientId = await CreateClientAsync();

            try
            {
                var code = NewCode();
                var unassignedCode = NewCode();
                await CreateCodeAsync(code, clientId);
                await CreateCodeAsync(unassignedCode);
                var basketId = Guid.NewGuid();
                await SaveSellerBasketAsync(basketId, clientId, code);

                var response = await _apiFixture.SellerWebClient.PostForResponseAsync(
                    ApiEndpoints.BasketApiEndpoint,
                    CreateSellerRequest(basketId, clientId, unassignedCode, lineCount: 2));

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                var persisted = await GetPersistedBasketAsync(basketId);
                Assert.Equal(code, persisted.DiscountCode);
                Assert.Single(persisted.Items);
            }
            finally
            {
                await DeleteClientAsync(clientId);
            }
        }

        [Fact]
        public async Task SellerSaveBasket_WithUnknownCode_IsRejectedWithTheSameMessageAsAnUnassignedOne()
        {
            var clientId = await CreateClientAsync();

            try
            {
                // A code that exists but is assigned to nobody.
                var code = NewCode();
                await CreateCodeAsync(code);

                var unassigned = await _apiFixture.SellerWebClient.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, CreateSellerRequest(Guid.NewGuid(), clientId, code));
                var unknown = await _apiFixture.SellerWebClient.PostForResponseAsync(ApiEndpoints.BasketApiEndpoint, CreateSellerRequest(Guid.NewGuid(), clientId, "UNKNOWN-CODE"));

                Assert.Equal(HttpStatusCode.BadRequest, unassigned.StatusCode);
                Assert.Equal(HttpStatusCode.BadRequest, unknown.StatusCode);
                Assert.Equal(await unassigned.Content.ReadAsStringAsync(), await unknown.Content.ReadAsStringAsync());
            }
            finally
            {
                await DeleteClientAsync(clientId);
            }
        }

        [Theory]
        [InlineData(Revocation.Unassigned)]
        [InlineData(Revocation.Deactivated)]
        [InlineData(Revocation.Deleted)]
        public async Task SellerSaveBasket_WhenTheStoredCodeStopsBeingValid_DropsItWithAMessage(Revocation revocation)
        {
            var clientId = await CreateClientAsync();

            try
            {
                var code = NewCode();
                var codeId = await CreateCodeAsync(code, clientId);
                var basketId = Guid.NewGuid();
                await SaveSellerBasketAsync(basketId, clientId, code);

                await RevokeAsync(revocation, codeId, code, clientId);

                // A blank code leaves the stored one unchanged, as the order form sends it.
                var saved = await SaveSellerBasketAsync(basketId, clientId, string.Empty);

                Assert.Null(saved.DiscountCode);
                Assert.False(string.IsNullOrWhiteSpace(saved.DiscountCodeRemovedMessage));
                Assert.Null((await GetPersistedBasketAsync(basketId)).DiscountCode);
            }
            finally
            {
                await DeleteClientAsync(clientId);
            }
        }

        [Fact]
        public async Task SellerUploadOrderFile_WithAValidCodeTypedInAnotherCase_StoresItsStoredSpelling()
        {
            var clientId = await CreateClientAsync();

            try
            {
                var sku = await CreateProductAsync();
                var code = NewCode();
                await CreateCodeAsync(code, clientId);
                var basketId = Guid.NewGuid();

                var response = await UploadOrderFileAsync(basketId, clientId, sku, code.ToLowerInvariant());

                await AssertOkAsync(response);
                Assert.Equal(code, (await ReadAsync<SellerBasketResponseModel>(response)).DiscountCode);
                Assert.Equal(code, (await GetPersistedBasketAsync(basketId)).DiscountCode);
            }
            finally
            {
                await DeleteClientAsync(clientId);
            }
        }

        [Fact]
        public async Task SellerUploadOrderFile_WithACodeNotValidForTheClient_IsRejectedAndLeavesTheBasketUnchanged()
        {
            var clientId = await CreateClientAsync();

            try
            {
                var sku = await CreateProductAsync();
                var code = NewCode();
                await CreateCodeAsync(code, clientId);
                var basketId = Guid.NewGuid();
                await SaveSellerBasketAsync(basketId, clientId, code);

                var response = await UploadOrderFileAsync(basketId, clientId, sku, "UNKNOWN-CODE");

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                var persisted = await GetPersistedBasketAsync(basketId);
                Assert.Equal(code, persisted.DiscountCode);
                Assert.DoesNotContain(persisted.Items, x => x.ProductSku == sku);
            }
            finally
            {
                await DeleteClientAsync(clientId);
            }
        }

        [Theory]
        [InlineData(Revocation.Unassigned)]
        [InlineData(Revocation.Deactivated)]
        [InlineData(Revocation.Deleted)]
        public async Task SellerUploadOrderFile_WhenTheStoredCodeStopsBeingValid_DropsItWithAMessage(Revocation revocation)
        {
            var clientId = await CreateClientAsync();

            try
            {
                var sku = await CreateProductAsync();
                var code = NewCode();
                var codeId = await CreateCodeAsync(code, clientId);
                var basketId = Guid.NewGuid();
                await SaveSellerBasketAsync(basketId, clientId, code);

                await RevokeAsync(revocation, codeId, code, clientId);

                // The upload omits the code, which leaves the stored one unchanged.
                var response = await UploadOrderFileAsync(basketId, clientId, sku, discountCode: null);

                await AssertOkAsync(response);
                var saved = await ReadAsync<SellerBasketResponseModel>(response);
                Assert.Null(saved.DiscountCode);
                Assert.False(string.IsNullOrWhiteSpace(saved.DiscountCodeRemovedMessage));
                Assert.Null((await GetPersistedBasketAsync(basketId)).DiscountCode);
            }
            finally
            {
                await DeleteClientAsync(clientId);
            }
        }

        [Fact]
        public async Task BuyerSaveBasket_AsATeamMember_AppliesTheClientsCode()
        {
            // The test user is a Team Member of the Client that shares its organisation but not its email.
            var client = await ClientHelper.GetOrCreateTestUsersClientAsync(_apiFixture);
            Assert.NotEqual(TestUserEmail, client.Email, StringComparer.OrdinalIgnoreCase);

            var code = NewCode();
            await CreateCodeAsync(code, client.Id.Value);

            var response = await _apiFixture.BuyerWebClient.PostForResponseAsync(
                ApiEndpoints.BasketApiEndpoint,
                new BuyerSaveBasketRequestModel
                {
                    DiscountCode = code.ToLowerInvariant(),
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

            await AssertOkAsync(response);
            Assert.Equal(code, (await ReadAsync<BuyerBasketResponseModel>(response)).DiscountCode);
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
                            ProductId = Guid.NewGuid(),
                            Sku = NewUnknownSku(),
                            Name = "Unpriced line",
                            Quantity = 1
                        }
                    }
                });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        public enum Revocation
        {
            Unassigned,
            Deactivated,
            Deleted
        }

        private async Task RevokeAsync(Revocation revocation, Guid codeId, string code, Guid clientId)
        {
            switch (revocation)
            {
                case Revocation.Unassigned:
                    await SaveCodeAsync(new DiscountCodeRequestModel { Id = codeId, Code = code, ClientIds = Array.Empty<Guid>() });
                    break;
                case Revocation.Deactivated:
                    await SaveCodeAsync(new DiscountCodeRequestModel { Id = codeId, Code = code, IsDisabled = true, ClientIds = new[] { clientId } });
                    break;
                case Revocation.Deleted:
                    var response = await _apiFixture.SellerWebClient.DeleteForResponseAsync($"{ApiEndpoints.DeleteDiscountCodeApiEndpoint}/{codeId}");
                    Assert.True(response.IsSuccessStatusCode, $"Deleting the code failed with {(int)response.StatusCode}.");
                    break;
            }
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
                CreateSellerRequest(basketId, clientId, discountCode));
        }

        private Task<SellerBasketApiResponseModel> GetPersistedBasketAsync(Guid basketId)
        {
            return _apiFixture.BasketApiClient.GetAsync<SellerBasketApiResponseModel>($"{ApiConstants.Baskets.BasketsApiEndpoint}/{basketId}");
        }

        private static SellerSaveBasketRequestModel CreateSellerRequest(Guid basketId, Guid clientId, string discountCode, int lineCount = 1)
        {
            return new SellerSaveBasketRequestModel
            {
                Id = basketId,
                ClientId = clientId,
                DiscountCode = discountCode,
                Items = Enumerable.Range(0, lineCount)
                    .Select(_ => new SellerBasketItemRequestModel
                    {
                        ProductId = Guid.NewGuid(),
                        Sku = NewUnknownSku(),
                        Name = "Unpriced line",
                        Quantity = 1
                    })
                    .ToList()
            };
        }

        private async Task<HttpResponseMessage> UploadOrderFileAsync(Guid basketId, Guid clientId, string sku, string discountCode)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet("Order");
            worksheet.Cell(1, 1).Value = "SKU";
            worksheet.Cell(1, 2).Value = "Quantity";
            worksheet.Cell(2, 1).Value = sku;
            worksheet.Cell(2, 2).Value = 1;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            var file = new ByteArrayContent(stream.ToArray());
            file.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            using var content = new MultipartFormDataContent
            {
                { file, "File", "order.xlsx" },
                { new StringContent(basketId.ToString()), "Id" },
                { new StringContent(clientId.ToString()), "ClientId" }
            };

            if (discountCode is not null)
            {
                content.Add(new StringContent(discountCode), "DiscountCode");
            }

            return await _apiFixture.SellerWebClient.PostFormForResponseAsync(ApiEndpoints.OrderFileApiEndpoint, content);
        }

        private async Task<string> CreateProductAsync()
        {
            var sku = $"ENF-{Guid.NewGuid():N}"[..20];

            await InventoryHelper.CreateProductAndAddToStockAsync(_apiFixture, new ProductRequestModel
            {
                Name = $"Enforcement {sku}",
                Sku = sku,
                CategoryId = Products.Lamica.CategoryId,
                IsPublished = true
            });

            return sku;
        }

        private async Task<Guid> CreateClientAsync()
        {
            var suffix = Guid.NewGuid().ToString("N");

            var created = await _apiFixture.ClientApiClient.PostAsync<ClientRequestModel, BaseResponseModel>(ClientsEndpoint, new ClientRequestModel
            {
                Name = $"Enforce-{suffix}",
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

        private static async Task AssertOkAsync(HttpResponseMessage response)
        {
            Assert.True(
                response.StatusCode == HttpStatusCode.OK,
                $"Expected 200 but got {(int)response.StatusCode}: {await response.Content.ReadAsStringAsync()}");
        }

        private static async Task<T> ReadAsync<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
    }
}
