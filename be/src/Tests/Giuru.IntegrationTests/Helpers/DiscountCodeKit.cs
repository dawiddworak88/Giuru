using Client.Api.v1.RequestModels;
using Client.Api.v1.ResponseModels;
using Foundation.ApiExtensions.Models.Response;
using Foundation.GenericRepository.Paginations;
using Giuru.IntegrationTests.Definitions;
using Giuru.IntegrationTests.HttpClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests.Helpers
{
    /// <summary>The catalog's own identity of the product the discount code tests put in their baskets.</summary>
    public sealed record DiscountCodeProduct(Guid Id, string Sku, string Name);

    /// <summary>A client created for one test: its own organisation and email, so no two tests share one.</summary>
    public sealed record TestClient(Guid Id, string Name, string Email, Guid OrganisationId);

    public sealed record SavedDiscountCode(Guid Id, string Version, string Code);

    /// <summary>
    /// The arrangement every discount code test needs: two sellers, clients created for the test alone, codes with
    /// unique text, and the requests to read and change them. Nothing here assumes a test is alone in the database.
    /// </summary>
    public sealed class DiscountCodeKit
    {
        private DiscountCodeKit(ApiFixture fixture, RestClient sellerApi, RestClient otherSellerApi, RestClient buyerApi)
        {
            Fixture = fixture;
            SellerApi = sellerApi;
            OtherSellerApi = otherSellerApi;
            BuyerApi = buyerApi;
        }

        public ApiFixture Fixture { get; }

        /// <summary>Client.Api, signed in as the first seller.</summary>
        public RestClient SellerApi { get; }

        /// <summary>Client.Api, signed in as a second seller of another organisation.</summary>
        public RestClient OtherSellerApi { get; }

        /// <summary>Client.Api, signed in as somebody who is no seller.</summary>
        public RestClient BuyerApi { get; }

        public static async Task<DiscountCodeKit> CreateAsync(ApiFixture fixture)
        {
            var sellerToken = await fixture.IssueSellerTokenAsync(ApiFixture.SellerEmail, ApiFixture.SellerOrganisationId);
            var otherSellerToken = await fixture.IssueSellerTokenAsync(ApiFixture.OtherSellerEmail, ApiFixture.OtherSellerOrganisationId);
            var buyerToken = await fixture.IssueBuyerTokenAsync($"api.buyer.{Guid.NewGuid():N}@dc.test", Guid.NewGuid());

            return new DiscountCodeKit(
                fixture,
                fixture.CreateClientApiClient(sellerToken),
                fixture.CreateClientApiClient(otherSellerToken),
                fixture.CreateClientApiClient(buyerToken));
        }

        /// <summary>A code nobody else uses, in upper case so that tests can tell the stored spelling from the typed one.</summary>
        public static string NewCode() => $"DC{Guid.NewGuid():N}".Substring(0, 20).ToUpperInvariant();

        public async Task<TestClient> CreateClientAsync(RestClient sellerApi = null, string namePrefix = "Client")
        {
            var suffix = Guid.NewGuid().ToString("N");
            var organisationId = Guid.NewGuid();
            var name = $"{namePrefix} {suffix}";
            var email = $"client.{suffix}@dc.test";

            var created = await (sellerApi ?? SellerApi).PostAsync<ClientRequestModel, BaseResponseModel>(ApiEndpoints.ClientApiClientsEndpoint, new ClientRequestModel
            {
                Name = name,
                Email = email,
                CommunicationLanguage = Clients.Language,
                OrganisationId = organisationId
            });

            Assert.NotNull(created.Id);

            return new TestClient(created.Id.Value, name, email, organisationId);
        }

        public Task<HttpResponseMessage> DeleteClientAsync(TestClient client, RestClient sellerApi = null)
        {
            return (sellerApi ?? SellerApi).DeleteForResponseAsync($"{ApiEndpoints.ClientApiClientsEndpoint}/{client.Id}");
        }

        public static DiscountCodeRequestModel Request(string code, IEnumerable<Guid> clientIds, bool isDisabled = false, string description = null, Guid? id = null, string version = null)
        {
            return new DiscountCodeRequestModel
            {
                Id = id,
                Version = version,
                Code = code,
                Description = description,
                IsDisabled = isDisabled,
                ClientIds = clientIds
            };
        }

        public Task<HttpResponseMessage> SaveAsync(DiscountCodeRequestModel request, RestClient api = null)
        {
            return (api ?? SellerApi).PostForResponseAsync(ApiEndpoints.ClientApiDiscountCodesEndpoint, request);
        }

        public async Task<SavedDiscountCode> CreateCodeAsync(string code, IEnumerable<Guid> clientIds = null, bool isDisabled = false, string description = null, RestClient api = null)
        {
            var response = await SaveAsync(Request(code, clientIds ?? Array.Empty<Guid>(), isDisabled, description), api);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            return await ReadSavedAsync(response, code);
        }

        /// <summary>Updates a code with everything about it stated again, as the form does.</summary>
        public async Task<HttpResponseMessage> UpdateAsync(SavedDiscountCode code, IEnumerable<Guid> clientIds, bool isDisabled = false, string newCode = null, string description = null, string version = null, RestClient api = null)
        {
            return await SaveAsync(Request(newCode ?? code.Code, clientIds, isDisabled, description, code.Id, version ?? code.Version), api);
        }

        public async Task<SavedDiscountCode> UpdateSuccessfullyAsync(SavedDiscountCode code, IEnumerable<Guid> clientIds, bool isDisabled = false, string newCode = null, RestClient api = null)
        {
            var response = await UpdateAsync(code, clientIds, isDisabled, newCode, api: api);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            return await ReadSavedAsync(response, newCode ?? code.Code);
        }

        public async Task<DiscountCodeResponseModel> GetAsync(Guid id, RestClient api = null)
        {
            return await (api ?? SellerApi).GetAsync<DiscountCodeResponseModel>($"{ApiEndpoints.ClientApiDiscountCodesEndpoint}/{id}");
        }

        public Task<HttpResponseMessage> GetForResponseAsync(Guid id, RestClient api = null)
        {
            return (api ?? SellerApi).GetForResponseAsync($"{ApiEndpoints.ClientApiDiscountCodesEndpoint}/{id}");
        }

        public async Task<IReadOnlyList<DiscountCodeResponseModel>> ListAsync(string searchTerm, RestClient api = null)
        {
            var page = await (api ?? SellerApi).GetAsync<PagedResults<IEnumerable<DiscountCodeResponseModel>>>(
                $"{ApiEndpoints.ClientApiDiscountCodesEndpoint}?searchTerm={Uri.EscapeDataString(searchTerm)}&pageIndex=1&itemsPerPage=100");

            return page.Data?.ToList() ?? new List<DiscountCodeResponseModel>();
        }

        public Task<HttpResponseMessage> DeleteAsync(Guid id, string version, RestClient api = null)
        {
            var query = version is null ? string.Empty : $"?version={Uri.EscapeDataString(version)}";

            return (api ?? SellerApi).DeleteForResponseAsync($"{ApiEndpoints.ClientApiDiscountCodesEndpoint}/{id}{query}");
        }

        public async Task<IReadOnlyList<DiscountCodeClientOptionResponseModel>> GetOptionsAsync(string searchTerm = null, RestClient api = null)
        {
            var options = new List<DiscountCodeClientOptionResponseModel>();
            var pageIndex = 1;

            while (true)
            {
                var query = $"?pageIndex={pageIndex}&itemsPerPage=50&orderBy={Uri.EscapeDataString("Name asc")}" + (searchTerm is null ? string.Empty : $"&searchTerm={Uri.EscapeDataString(searchTerm)}");
                var page = await (api ?? SellerApi).GetAsync<PagedResults<IEnumerable<DiscountCodeClientOptionResponseModel>>>($"{ApiEndpoints.ClientApiDiscountCodeClientsEndpoint}{query}");

                options.AddRange(page.Data ?? Enumerable.Empty<DiscountCodeClientOptionResponseModel>());

                if (pageIndex >= page.PageCount)
                {
                    return options;
                }

                pageIndex++;
            }
        }

        public Task<HttpResponseMessage> ValidateAsync(RestClient api, string code, Guid clientId)
        {
            return api.GetForResponseAsync($"{ApiEndpoints.ClientApiDiscountCodeValidationEndpoint}?code={Uri.EscapeDataString(code)}&clientId={clientId}");
        }

        public async Task<(string Status, string DiscountCode)> ValidateOkAsync(RestClient api, string code, Guid clientId)
        {
            var response = await ValidateAsync(api, code, clientId);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await RestClient.ReadAsync<DiscountCodeValidationResponseModel>(response);

            return (body.Status, body.DiscountCode);
        }

        /// <summary>A buyer's web client: signed in as the client's own contact, with a basket of its own.</summary>
        public async Task<RestClient> CreateBuyerWebClientAsync(TestClient client)
        {
            return Fixture.CreateBuyerWebClient(await Fixture.IssueBuyerTokenAsync(client.Email, client.OrganisationId));
        }

        /// <summary>A buyer's web client signed in as somebody of the client's team: a distinct email, the client's organisation.</summary>
        public async Task<RestClient> CreateTeamMemberWebClientAsync(TestClient client)
        {
            return Fixture.CreateBuyerWebClient(await Fixture.IssueBuyerTokenAsync($"team.{Guid.NewGuid():N}@dc.test", client.OrganisationId));
        }

        private static async Task<SavedDiscountCode> ReadSavedAsync(HttpResponseMessage response, string code)
        {
            var body = await RestClient.ReadAsync<SaveResult>(response);

            Assert.NotNull(body.Id);
            Assert.False(string.IsNullOrWhiteSpace(body.Version));

            return new SavedDiscountCode(body.Id.Value, body.Version, code);
        }

        private sealed class SaveResult
        {
            public Guid? Id { get; set; }
            public string Version { get; set; }
        }
    }
}
