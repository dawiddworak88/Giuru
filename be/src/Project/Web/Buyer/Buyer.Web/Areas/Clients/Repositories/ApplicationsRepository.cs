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
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _settings;

        public ApplicationsRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            _apiClientService = apiClientService;
            _settings = settings;
        }
            
        public async Task CreateClientApplicationAsync(string token, string language, string companyName, string firstName,string lastName,
            string contactJobTitle, string email, string phoneNumber, string communicationLanguage, bool isDeliveryAddressEqualBillingAddress,
            ClientApplicationAddressRequestModel billingAddress, ClientApplicationAddressRequestModel deliveryAddress)
        { 

            var requestModel = new ClientApplicationRequestModel
            {
                CompanyName = companyName,
                FirstName = firstName,
                LastName = lastName,
                ContactJobTitle = contactJobTitle,
                Email = email,
                PhoneNumber = phoneNumber,
                CommunicationLanguage = communicationLanguage,
                IsDeliveryAddressEqualBillingAddress = isDeliveryAddressEqualBillingAddress,
                BillingAddress = billingAddress,
                DeliveryAddress = deliveryAddress
            };

            var apiRequest = new ApiRequest<ClientApplicationRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.ApplicationsApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<ClientApplicationRequestModel>, ClientApplicationRequestModel, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }
    }
}
