using Client.Api.v1.RequestModels;
using Client.Api.v1.ResponseModels;
using Foundation.ApiExtensions.Models.Response;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Paginations;
using Giuru.IntegrationTests.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
    // Discount Code management is exercised against Client.Api directly: the Seller portal proxy
    // that fronts it arrives in a later pull request. Clients created here are deleted again so
    // that tests sharing the fixture keep seeing only their own Clients.
    [Collection(nameof(ApiCollection))]
    public class DiscountCodeManagementTests
    {
        private const string ClientsEndpoint = "api/v1/Clients";

        private readonly ApiFixture _apiFixture;

        public DiscountCodeManagementTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        [Fact]
        public async Task SaveAndGet_StoresTrimmedCodeDescriptionActiveSwitchAndAssignedClients()
        {
            var code = NewCode();
            var clientId = await CreateClientAsync();

            try
            {
                var created = await SaveAsync(new DiscountCodeRequestModel
                {
                    Code = $"  {code}  ",
                    Description = "Spring offer",
                    IsDisabled = false,
                    ClientIds = new[] { clientId }
                });

                var discountCode = await GetByIdAsync(created.Id);

                Assert.Equal(code, discountCode.Code);
                Assert.Equal("Spring offer", discountCode.Description);
                Assert.False(discountCode.IsDisabled);
                Assert.Equal(new[] { clientId }, discountCode.ClientIds);
                Assert.Equal(1, discountCode.ClientsCount);
            }
            finally
            {
                await DeleteClientAsync(clientId);
            }
        }

        [Fact]
        public async Task Update_ChangesDescriptionActiveSwitchAndAssignedClients_KeepsCode()
        {
            var code = NewCode();
            var firstClientId = await CreateClientAsync();
            var secondClientId = await CreateClientAsync();

            try
            {
                var created = await SaveAsync(new DiscountCodeRequestModel { Code = code, ClientIds = new[] { firstClientId } });

                await SaveAsync(new DiscountCodeRequestModel
                {
                    Id = created.Id,
                    Code = code,
                    Description = "Paused",
                    IsDisabled = true,
                    ClientIds = new[] { secondClientId }
                });

                var discountCode = await GetByIdAsync(created.Id);

                Assert.Equal(code, discountCode.Code);
                Assert.Equal("Paused", discountCode.Description);
                Assert.True(discountCode.IsDisabled);
                Assert.Equal(new[] { secondClientId }, discountCode.ClientIds);
            }
            finally
            {
                await DeleteClientAsync(firstClientId);
                await DeleteClientAsync(secondClientId);
            }
        }

        [Fact]
        public async Task Save_WithoutAssignedClients_IsAllowed()
        {
            var created = await SaveAsync(new DiscountCodeRequestModel { Code = NewCode() });

            var discountCode = await GetByIdAsync(created.Id);

            Assert.Empty(discountCode.ClientIds);
            Assert.Equal(0, discountCode.ClientsCount);
        }

        [Theory]
        [InlineData("ab")]
        [InlineData("has space")]
        [InlineData("semi;colon")]
        [InlineData("zażółć")]
        public async Task Save_WithInvalidFormat_IsRejected(string code)
        {
            var response = await _apiFixture.ClientApiClient.PostForResponseAsync(ApiEndpoints.DiscountCodesClientApiEndpoint, new DiscountCodeRequestModel { Code = code });

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task Save_WithCodeLongerThanFiftyCharacters_IsRejected()
        {
            var response = await _apiFixture.ClientApiClient.PostForResponseAsync(ApiEndpoints.DiscountCodesClientApiEndpoint, new DiscountCodeRequestModel { Code = new string('A', 51) });

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task Save_WithSameCodeIgnoringCase_IsRejectedAsConflict()
        {
            var code = NewCode();
            await SaveAsync(new DiscountCodeRequestModel { Code = code });

            var response = await _apiFixture.ClientApiClient.PostForResponseAsync(ApiEndpoints.DiscountCodesClientApiEndpoint, new DiscountCodeRequestModel { Code = code.ToLowerInvariant() });

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task Save_WithCodeOfInactiveDiscountCode_IsStillRejectedAsConflict()
        {
            var code = NewCode();
            await SaveAsync(new DiscountCodeRequestModel { Code = code, IsDisabled = true });

            var response = await _apiFixture.ClientApiClient.PostForResponseAsync(ApiEndpoints.DiscountCodesClientApiEndpoint, new DiscountCodeRequestModel { Code = code });

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task Save_WithCodeOfDeletedDiscountCode_IsAllowed()
        {
            var code = NewCode();
            var first = await SaveAsync(new DiscountCodeRequestModel { Code = code });

            var deleteResponse = await _apiFixture.ClientApiClient.DeleteForResponseAsync($"{ApiEndpoints.DiscountCodesClientApiEndpoint}/{first.Id}");
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

            var second = await SaveAsync(new DiscountCodeRequestModel { Code = code });

            Assert.NotEqual(first.Id, second.Id);
        }

        [Fact]
        public async Task Update_WithDifferentCodeText_IsRejected()
        {
            var created = await SaveAsync(new DiscountCodeRequestModel { Code = NewCode() });

            var response = await _apiFixture.ClientApiClient.PostForResponseAsync(ApiEndpoints.DiscountCodesClientApiEndpoint, new DiscountCodeRequestModel { Id = created.Id, Code = NewCode() });

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task Update_WithSameCodeInDifferentCase_IsRejected()
        {
            var code = NewCode();
            var created = await SaveAsync(new DiscountCodeRequestModel { Code = code });

            var response = await _apiFixture.ClientApiClient.PostForResponseAsync(ApiEndpoints.DiscountCodesClientApiEndpoint, new DiscountCodeRequestModel { Id = created.Id, Code = code.ToLowerInvariant() });

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task Save_WithUnknownClientId_IsRejected()
        {
            var response = await _apiFixture.ClientApiClient.PostForResponseAsync(ApiEndpoints.DiscountCodesClientApiEndpoint, new DiscountCodeRequestModel { Code = NewCode(), ClientIds = new[] { Guid.NewGuid() } });

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task Save_WithDeletedClientId_IsRejected()
        {
            var clientId = await CreateClientAsync();
            await DeleteClientAsync(clientId);

            var response = await _apiFixture.ClientApiClient.PostForResponseAsync(ApiEndpoints.DiscountCodesClientApiEndpoint, new DiscountCodeRequestModel { Code = NewCode(), ClientIds = new[] { clientId } });

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task List_SearchesByCodeAndReportsAssignedClientsCount()
        {
            var prefix = $"LST{Guid.NewGuid():N}"[..12];
            var clientId = await CreateClientAsync();

            try
            {
                await SaveAsync(new DiscountCodeRequestModel { Code = $"{prefix}-A", ClientIds = new[] { clientId } });
                await SaveAsync(new DiscountCodeRequestModel { Code = $"{prefix}-B" });
                await SaveAsync(new DiscountCodeRequestModel { Code = NewCode() });

                var results = await _apiFixture.ClientApiClient.GetAsync<PagedResults<IEnumerable<DiscountCodeResponseModel>>>(
                    $"{ApiEndpoints.DiscountCodesClientApiEndpoint}?searchTerm={prefix}&pageIndex={Constants.DefaultPageIndex}&itemsPerPage={Constants.DefaultItemsPerPage}");

                Assert.Equal(2, results.Total);
                Assert.Equal(1, results.Data.Single(x => x.Code == $"{prefix}-A").ClientsCount);
                Assert.Equal(0, results.Data.Single(x => x.Code == $"{prefix}-B").ClientsCount);
            }
            finally
            {
                await DeleteClientAsync(clientId);
            }
        }

        [Fact]
        public async Task Delete_RemovesCodeFromGet()
        {
            var created = await SaveAsync(new DiscountCodeRequestModel { Code = NewCode() });

            var deleteResponse = await _apiFixture.ClientApiClient.DeleteForResponseAsync($"{ApiEndpoints.DiscountCodesClientApiEndpoint}/{created.Id}");
            var getResponse = await _apiFixture.ClientApiClient.GetForResponseAsync($"{ApiEndpoints.DiscountCodesClientApiEndpoint}/{created.Id}");

            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task DeletingAssignedClient_DropsItFromTheDiscountCodesAssignedClients()
        {
            var clientId = await CreateClientAsync();
            var created = await SaveAsync(new DiscountCodeRequestModel { Code = NewCode(), ClientIds = new[] { clientId } });

            await DeleteClientAsync(clientId);

            var discountCode = await GetByIdAsync(created.Id);

            Assert.Empty(discountCode.ClientIds);
            Assert.Equal(0, discountCode.ClientsCount);
        }

        private static string NewCode()
        {
            return $"T{Guid.NewGuid():N}"[..16];
        }

        private async Task<BaseResponseModel> SaveAsync(DiscountCodeRequestModel request)
        {
            return await _apiFixture.ClientApiClient.PostAsync<DiscountCodeRequestModel, BaseResponseModel>(ApiEndpoints.DiscountCodesClientApiEndpoint, request);
        }

        private async Task<DiscountCodeResponseModel> GetByIdAsync(Guid? id)
        {
            return await _apiFixture.ClientApiClient.GetAsync<DiscountCodeResponseModel>($"{ApiEndpoints.DiscountCodesClientApiEndpoint}/{id}");
        }

        private async Task<Guid> CreateClientAsync()
        {
            var suffix = Guid.NewGuid().ToString("N");

            var created = await _apiFixture.ClientApiClient.PostAsync<ClientRequestModel, BaseResponseModel>(ClientsEndpoint, new ClientRequestModel
            {
                Name = $"Discount-{suffix}",
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
