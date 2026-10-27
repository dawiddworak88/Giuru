using Foundation.ApiExtensions.Models.Response;
using Giuru.IntegrationTests.Definitions;
using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ClientRequestModel = Client.Api.v1.RequestModels.ClientRequestModel;
using ClientResponseModel = Client.Api.v1.ResponseModels.ClientResponseModel;

namespace Giuru.IntegrationTests.Helpers
{
    public static class ClientHelper
    {
        private const string ClientsEndpoint = "api/v1/Clients";
        private const string ClientByOrganisationEndpoint = "api/v1/Clients/organisation";

        /// <summary>
        /// The buyer portal resolves the test user's Client by organisation and caches it, so this Client is
        /// reused when another test already created one, and is never deleted by the tests.
        /// </summary>
        public static async Task<ClientResponseModel> GetOrCreateTestUsersClientAsync(ApiFixture apiFixture)
        {
            var response = await apiFixture.ClientApiClient.GetForResponseAsync(ClientByOrganisationEndpoint);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ClientResponseModel>(await response.Content.ReadAsStringAsync());
            }

            var suffix = Guid.NewGuid().ToString("N");
            var email = $"{suffix}@team-member-tests.com";

            var created = await apiFixture.ClientApiClient.PostAsync<ClientRequestModel, BaseResponseModel>(ClientsEndpoint, new ClientRequestModel
            {
                Name = $"TeamMember-{suffix}",
                Email = email,
                CommunicationLanguage = Clients.Language,
                OrganisationId = Clients.OrganisationId
            });

            return new ClientResponseModel { Id = created.Id, Email = email };
        }

        /// <summary>A Client in an organisation of its own, for tests to delete when they finish.</summary>
        public static async Task<Guid> CreateClientAsync(ApiFixture apiFixture, string namePrefix)
        {
            var suffix = Guid.NewGuid().ToString("N");

            var created = await apiFixture.ClientApiClient.PostAsync<ClientRequestModel, BaseResponseModel>(ClientsEndpoint, new ClientRequestModel
            {
                Name = $"{namePrefix}-{suffix}",
                Email = $"{suffix}@discount-tests.com",
                CommunicationLanguage = Clients.Language,
                OrganisationId = Guid.NewGuid()
            });

            return created.Id.Value;
        }

        public static async Task DeleteClientAsync(ApiFixture apiFixture, Guid clientId)
        {
            await apiFixture.ClientApiClient.DeleteForResponseAsync($"{ClientsEndpoint}/{clientId}");
        }
    }
}
