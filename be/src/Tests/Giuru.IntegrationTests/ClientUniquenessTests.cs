using Client.Api.v1.RequestModels;
using Client.Api.v1.ResponseModels;
using Giuru.IntegrationTests.Definitions;
using Giuru.IntegrationTests.Helpers;
using Giuru.IntegrationTests.HttpClients;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
    /// <summary>
    /// A buyer is matched to their client by email, or by organisation for a member of the client's team. Client.Api
    /// keeps both to one active client, or a buyer would price and order as whichever of two clients a lookup found.
    /// </summary>
    [Collection(nameof(ApiCollection))]
    public class ClientUniquenessTests
    {
        private readonly ApiFixture _apiFixture;

        public ClientUniquenessTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        private static ClientRequestModel Request(Guid? id, string email, Guid organisationId)
        {
            return new ClientRequestModel
            {
                Id = id,
                Name = $"Client {Guid.NewGuid():N}",
                Email = email,
                CommunicationLanguage = Clients.Language,
                OrganisationId = organisationId
            };
        }

        // The seller app looks the organisation up by the email, so the email of another client comes with its organisation.
        [Fact]
        public async Task Update_WithTheEmailOfAnotherClient_IsRefusedAndLeavesTheClientAsItWas()
        {
            var kit = await DiscountCodeKit.CreateAsync(_apiFixture);
            var client = await kit.CreateClientAsync();
            var other = await kit.CreateClientAsync();

            var response = await kit.SellerApi.PostForResponseAsync(ApiEndpoints.ClientApiClientsEndpoint, Request(client.Id, other.Email, other.OrganisationId));

            await RestClient.AssertStatusAsync(response, HttpStatusCode.Conflict);

            var read = await kit.SellerApi.GetAsync<ClientResponseModel>($"{ApiEndpoints.ClientApiClientsEndpoint}/{client.Id}");

            Assert.Equal(client.Email, read.Email);
            Assert.Equal(client.OrganisationId, read.OrganisationId);
        }

        [Fact]
        public async Task Update_IntoTheOrganisationOfAnotherClient_IsRefused()
        {
            var kit = await DiscountCodeKit.CreateAsync(_apiFixture);
            var client = await kit.CreateClientAsync();
            var other = await kit.CreateClientAsync();

            var response = await kit.SellerApi.PostForResponseAsync(ApiEndpoints.ClientApiClientsEndpoint, Request(client.Id, client.Email, other.OrganisationId));

            await RestClient.AssertStatusAsync(response, HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Create_InTheOrganisationOfAnotherClient_IsRefused()
        {
            var kit = await DiscountCodeKit.CreateAsync(_apiFixture);
            var other = await kit.CreateClientAsync();

            var response = await kit.SellerApi.PostForResponseAsync(ApiEndpoints.ClientApiClientsEndpoint, Request(null, $"client.{Guid.NewGuid():N}@dc.test", other.OrganisationId));

            await RestClient.AssertStatusAsync(response, HttpStatusCode.Conflict);
        }

        // The email and organisation a client already has are its own, not another client's.
        [Fact]
        public async Task Update_KeepingItsOwnEmailAndOrganisation_IsSaved()
        {
            var kit = await DiscountCodeKit.CreateAsync(_apiFixture);
            var client = await kit.CreateClientAsync();

            var response = await kit.SellerApi.PostForResponseAsync(ApiEndpoints.ClientApiClientsEndpoint, Request(client.Id, client.Email, client.OrganisationId));

            await RestClient.AssertStatusAsync(response, HttpStatusCode.OK);
        }

        // A deleted client keeps its email and organisation in the database, but no longer holds them.
        [Fact]
        public async Task Update_WithTheEmailOfADeletedClient_IsSaved()
        {
            var kit = await DiscountCodeKit.CreateAsync(_apiFixture);
            var client = await kit.CreateClientAsync();
            var deleted = await kit.CreateClientAsync();
            await RestClient.AssertStatusAsync(await kit.DeleteClientAsync(deleted), HttpStatusCode.OK);

            var response = await kit.SellerApi.PostForResponseAsync(ApiEndpoints.ClientApiClientsEndpoint, Request(client.Id, deleted.Email, deleted.OrganisationId));

            await RestClient.AssertStatusAsync(response, HttpStatusCode.OK);
        }
    }
}
