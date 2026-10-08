using Client.Api.v1.RequestModels;
using Client.Api.v1.ResponseModels;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.GenericRepository.Paginations;
using Giuru.IntegrationTests.HttpClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests.Helpers
{
    /// <summary>
    /// A seller of its own: a fresh organisation and so a fresh set of codes and clients, which isolates every test
    /// from the data of the others without any clean-up. Everything is seeded straight through Client.Api, because
    /// Seller.Web cannot verify a code against the unreachable Grula.
    /// </summary>
    public sealed class DiscountSeller
    {
        private readonly ApiFixture _fixture;

        private DiscountSeller(ApiFixture fixture, Guid organisationId, string email, string token)
        {
            _fixture = fixture;
            OrganisationId = organisationId;
            Email = email;
            Token = token;
            ClientApi = fixture.CreateClientApiClient(token);
        }

        public Guid OrganisationId { get; }
        public string Email { get; }
        public string Token { get; }
        public RestClient ClientApi { get; }

        public static async Task<DiscountSeller> CreateAsync(ApiFixture fixture)
        {
            var organisationId = Guid.NewGuid();
            var email = $"seller-{organisationId:N}@tests.com";

            return new DiscountSeller(fixture, organisationId, email, await fixture.GetTokenAsync(email, "Seller", organisationId));
        }

        public RestClient CreateEnforcedSellerWebClient() => _fixture.CreateEnforcedSellerWebClient(Token);

        public async Task<Guid> CreateCodeAsync(string code, bool isDisabled = false, string description = null)
        {
            var response = await ClientApi.PostAsync<DiscountCodeRequestModel, BaseResponseModel>(
                ApiConstants.Client.DiscountCodesApiEndpoint,
                new DiscountCodeRequestModel { Code = code, Description = description, IsDisabled = isDisabled });

            return response.Id.Value;
        }

        public Task<HttpResponseMessage> TryCreateCodeAsync(string code, string description = null)
        {
            return ClientApi.PostForResponseAsync(
                ApiConstants.Client.DiscountCodesApiEndpoint,
                new DiscountCodeRequestModel { Code = code, Description = description });
        }

        public Task<HttpResponseMessage> UpdateCodeAsync(Guid id, string description, bool isDisabled, string code = null)
        {
            return ClientApi.PostForResponseAsync(
                ApiConstants.Client.DiscountCodesApiEndpoint,
                new DiscountCodeRequestModel { Id = id, Code = code, Description = description, IsDisabled = isDisabled });
        }

        public Task<HttpResponseMessage> DeleteCodeAsync(Guid id)
        {
            return ClientApi.DeleteForResponseAsync($"{ApiConstants.Client.DiscountCodesApiEndpoint}/{id}");
        }

        public async Task<IReadOnlyList<DiscountCodeResponseModel>> ListCodesAsync()
        {
            var page = await ClientApi.GetAsync<PagedResults<IEnumerable<DiscountCodeResponseModel>>>(
                $"{ApiConstants.Client.DiscountCodesApiEndpoint}?pageIndex=1&itemsPerPage=100");

            return page.Data.ToList();
        }

        /// <summary>Creates a client of this seller. <paramref name="discountCodeIds"/> null creates no assignments.</summary>
        public async Task<DiscountClient> CreateClientAsync(IEnumerable<Guid> discountCodeIds = null, Guid? buyerOrganisationId = null)
        {
            var organisationId = buyerOrganisationId ?? Guid.NewGuid();
            var email = $"buyer-{Guid.NewGuid():N}@tests.com";

            var response = await ClientApi.PostAsync<ClientRequestModel, BaseResponseModel>(
                ApiConstants.Client.ClientsApiEndpoint,
                new ClientRequestModel
                {
                    Name = $"Client {email}",
                    Email = email,
                    CommunicationLanguage = "en",
                    OrganisationId = organisationId,
                    DiscountCodeIds = discountCodeIds
                });

            return new DiscountClient(_fixture, response.Id.Value, email, organisationId, this);
        }

        public Task<HttpResponseMessage> SaveClientAsync(ClientRequestModel request)
        {
            return ClientApi.PostForResponseAsync(ApiConstants.Client.ClientsApiEndpoint, request);
        }

        public Task<ClientResponseModel> GetClientAsync(Guid clientId)
        {
            return ClientApi.GetAsync<ClientResponseModel>($"{ApiConstants.Client.ClientsApiEndpoint}/{clientId}");
        }

        public Task<HttpResponseMessage> ValidateAsync(string code, Guid clientId)
        {
            return ClientApi.GetForResponseAsync(ValidationUrl(code, clientId));
        }

        public static string ValidationUrl(string code, Guid clientId)
        {
            return $"{ApiConstants.Client.DiscountCodesValidationApiEndpoint}?code={Uri.EscapeDataString(code ?? string.Empty)}&clientId={clientId}";
        }
    }

    /// <summary>A client of a <see cref="DiscountSeller"/>, able to act as the buyer behind it.</summary>
    public sealed class DiscountClient
    {
        private readonly ApiFixture _fixture;

        public DiscountClient(ApiFixture fixture, Guid id, string email, Guid organisationId, DiscountSeller seller)
        {
            _fixture = fixture;
            Id = id;
            Email = email;
            OrganisationId = organisationId;
            Seller = seller;
        }

        public Guid Id { get; }
        public string Email { get; }
        public Guid OrganisationId { get; }
        public DiscountSeller Seller { get; }

        /// <summary>The token of the buyer behind this client: its email and organisation, and no Seller role.</summary>
        public Task<string> GetBuyerTokenAsync() => _fixture.GetTokenAsync(Email, "none", OrganisationId);

        public async Task<RestClient> CreateBuyerClientApiAsync() => _fixture.CreateClientApiClient(await GetBuyerTokenAsync());

        public async Task<RestClient> CreateEnforcedBuyerWebClientAsync() => _fixture.CreateEnforcedBuyerWebClient(await GetBuyerTokenAsync());

        public async Task<ValidationAnswer> ValidateAsSellerAsync(string code)
        {
            var response = await Seller.ValidateAsync(code, Id);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            return await ValidationAnswer.ReadAsync(response);
        }

        public async Task<ValidationAnswer> ValidateAsBuyerAsync(string code)
        {
            var buyerClientApi = await CreateBuyerClientApiAsync();
            var response = await buyerClientApi.GetForResponseAsync(DiscountSeller.ValidationUrl(code, Id));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            return await ValidationAnswer.ReadAsync(response);
        }
    }

    public sealed class ValidationAnswer
    {
        public string Status { get; init; }
        public string DiscountCode { get; init; }

        public static async Task<ValidationAnswer> ReadAsync(HttpResponseMessage response)
        {
            var model = await RestClient.ReadAsync<DiscountCodeValidationResponseModel>(response);

            return new ValidationAnswer { Status = model.Status, DiscountCode = model.DiscountCode };
        }
    }
}
