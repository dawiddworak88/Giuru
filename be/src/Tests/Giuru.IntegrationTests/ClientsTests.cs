using Client.Api.v1.RequestModels;
using Client.Api.v1.ResponseModels;
using Foundation.ApiExtensions.Models.Response;
using Giuru.IntegrationTests.Definitions;
using System;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
    [Collection(nameof(ApiCollection))]
    public class ClientsTests
    {
        private readonly ApiFixture _apiFixture;

        public ClientsTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        [Fact]
        public async Task CreateUpdateGet_ReturnsClient()
        {
            var endpointUrl = $"http://{_apiFixture._clientApiContainer.Hostname}:{_apiFixture._clientApiContainer.GetMappedPublicPort(8080)}{ApiEndpoints.ClientsApiEndpoint}";

            var createResult = await _apiFixture.RestClient.PostAsync<ClientRequestModel, BaseResponseModel>(endpointUrl, new ClientRequestModel
            {
                Name = Clients.Name,
                Email = Clients.Email,
                CommunicationLanguage = Clients.Language,
                OrganisationId = Clients.OrganisationId
            });

            Assert.NotNull(createResult);
            Assert.NotEqual(Guid.Empty, createResult.Data?.Id);

            var updateResult = await _apiFixture.RestClient.PostAsync<ClientRequestModel, BaseResponseModel>(endpointUrl, new ClientRequestModel
            {
                Id = createResult.Data?.Id,
                Name = Clients.UpdatedName,
                Email = Clients.Email,
                CommunicationLanguage = Clients.Language,
                OrganisationId = Clients.OrganisationId
            });

            Assert.NotNull(updateResult);
            Assert.Equal(createResult.Data?.Id, updateResult.Data?.Id);

            var getResult = await _apiFixture.RestClient.GetAsync<ClientResponseModel>($"{endpointUrl}/{updateResult.Data?.Id}");

            Assert.NotNull(getResult);
            Assert.Equal(updateResult.Data?.Id, getResult.Id);
            Assert.Equal(Clients.UpdatedName, getResult.Name);
        }
    }
}
