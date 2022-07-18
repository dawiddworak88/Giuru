using Buyer.Web.Shared.ApiRequestModels.Application;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Clients.Repositories
{
    public class ApplicationsRepository : IApplicationsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public ApplicationsRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
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
