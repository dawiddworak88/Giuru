using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Foundation.ApiExtensions.Models.Request;
using Foundation.Extensions.Exceptions;
using Foundation.ApiExtensions.Shared.Definitions;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.DomainModels.Clients;
using Buyer.Web.Shared.ApiRequestModels.Application;
using Foundation.ApiExtensions.Models.Response;

namespace Buyer.Web.Shared.Repositories.Clients
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public ClientsRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<Client> GetClientAsync(string token, string language)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.ClientUrl}{ApiConstants.Identity.ClientByOrganisationApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, Client>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return response.Data;
            }

            return default;
        }

        public async Task CreateClientApplicationAsync(
            string token, string language, string firstName, string lastName, string contactJobTitle, string email, string phoneNumber, string companyName,
            string companyAddress, string companyCountry, string companyCity, string companyRegion, string companyPostalCode)
        {
            var requestModel = new ClientApplicationRequestModel
            {
                FirstName = firstName,
                LastName = lastName,
                ContactJobTitle = contactJobTitle,
                Email = email,
                PhoneNumber = phoneNumber,
                CompanyName = companyName,
                CompanyAddress = companyAddress,
                CompanyCountry = companyCountry,
                CompanyCity = companyCity,
                CompanyRegion = companyRegion,
                CompanyPostalCode = companyPostalCode
            };

            var apiRequest = new ApiRequest<ClientApplicationRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.ClientUrl}{ApiConstants.Client.ApplicationsApiEndpoint}"
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<ClientApplicationRequestModel>, ClientApplicationRequestModel, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }
    }
}
