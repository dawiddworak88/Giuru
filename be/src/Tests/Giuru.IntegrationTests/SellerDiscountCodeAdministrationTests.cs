using Client.Api.v1.RequestModels;
using Foundation.GenericRepository.Paginations;
using Giuru.IntegrationTests.Definitions;
using Giuru.IntegrationTests.Helpers;
using Giuru.IntegrationTests.HttpClients;
using Seller.Web.Areas.Clients.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
    /// <summary>The management endpoints of the seller app, which sit in front of Client.Api and are available while Grula is configured.</summary>
    [Collection(nameof(ApiCollection))]
    public class SellerDiscountCodeAdministrationTests
    {
        private readonly ApiFixture _apiFixture;

        public SellerDiscountCodeAdministrationTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        private sealed class SaveResponse
        {
            public Guid? Id { get; set; }
            public string Version { get; set; }
            public string Message { get; set; }
        }

        private static DiscountCodeRequestModel Request(string code, IEnumerable<Guid> clientIds, Guid? id = null, string version = null, bool isDisabled = false)
        {
            return DiscountCodeKit.Request(code, clientIds, isDisabled, null, id, version);
        }

        [Fact]
        public async Task SaveListUpdateAndDelete_RoundTripThroughTheSellerApp()
        {
            var kit = await DiscountCodeKit.CreateAsync(_apiFixture);
            var web = _apiFixture.SellerWebClient;
            var client = await kit.CreateClientAsync();
            var codeText = DiscountCodeKit.NewCode();

            var createResponse = await web.PostForResponseAsync(ApiEndpoints.DiscountCodesWebApiEndpoint, Request(codeText, new[] { client.Id }));
            Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);
            var created = await RestClient.ReadAsync<SaveResponse>(createResponse);

            Assert.NotNull(created.Id);
            Assert.False(string.IsNullOrWhiteSpace(created.Version));
            Assert.Equal("Discount code saved", created.Message);

            var list = await web.GetAsync<PagedResults<IEnumerable<DiscountCode>>>($"{ApiEndpoints.GetDiscountCodesWebApiEndpoint}?searchTerm={codeText}&pageIndex=1&itemsPerPage=25");
            var listed = Assert.Single(list.Data);

            Assert.Equal(created.Id, listed.Id);
            Assert.Equal(1, listed.ClientsCount);
            Assert.Equal(created.Version, listed.Version);
            Assert.False(listed.IsDisabled);

            var updateResponse = await web.PostForResponseAsync(ApiEndpoints.DiscountCodesWebApiEndpoint, Request(codeText, Array.Empty<Guid>(), created.Id, created.Version, isDisabled: true));
            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
            var updated = await RestClient.ReadAsync<SaveResponse>(updateResponse);

            Assert.NotEqual(created.Version, updated.Version);

            var staleDelete = await web.DeleteForResponseAsync($"{ApiEndpoints.DeleteDiscountCodeWebApiEndpoint}?id={created.Id}&version={Uri.EscapeDataString(created.Version)}");

            Assert.Equal(HttpStatusCode.Conflict, staleDelete.StatusCode);
            Assert.Equal("This discount code changed since you opened it. Reload before saving or deleting", await RestClient.ReadMessageAsync(staleDelete));

            var relisted = await web.GetAsync<PagedResults<IEnumerable<DiscountCode>>>($"{ApiEndpoints.GetDiscountCodesWebApiEndpoint}?searchTerm={codeText}&pageIndex=1&itemsPerPage=25");

            Assert.Single(relisted.Data);
            Assert.True(relisted.Data.Single().IsDisabled);

            var delete = await web.DeleteForResponseAsync($"{ApiEndpoints.DeleteDiscountCodeWebApiEndpoint}?id={created.Id}&version={Uri.EscapeDataString(updated.Version)}");

            Assert.Equal(HttpStatusCode.OK, delete.StatusCode);
            Assert.Equal("Discount code deleted", await RestClient.ReadMessageAsync(delete));
            Assert.Empty(await kit.ListAsync(codeText));
        }

        [Fact]
        public async Task Save_OfATextThatAlreadyExists_SurfacesTheConflictToTheUser()
        {
            var kit = await DiscountCodeKit.CreateAsync(_apiFixture);
            var codeText = DiscountCodeKit.NewCode();
            await kit.CreateCodeAsync(codeText);

            var response = await _apiFixture.SellerWebClient.PostForResponseAsync(ApiEndpoints.DiscountCodesWebApiEndpoint, Request(codeText, Array.Empty<Guid>()));

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            Assert.Equal("A discount code with this value already exists", await RestClient.ReadMessageAsync(response));
        }

        [Fact]
        public async Task Save_WithAClientOfAnotherSeller_SurfacesTheRefusalToTheUser()
        {
            var kit = await DiscountCodeKit.CreateAsync(_apiFixture);
            var foreign = await kit.CreateClientAsync(kit.OtherSellerApi);

            var response = await _apiFixture.SellerWebClient.PostForResponseAsync(ApiEndpoints.DiscountCodesWebApiEndpoint, Request(DiscountCodeKit.NewCode(), new[] { foreign.Id }));

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("One or more selected clients are unavailable for assignment", await RestClient.ReadMessageAsync(response));
        }
    }
}
