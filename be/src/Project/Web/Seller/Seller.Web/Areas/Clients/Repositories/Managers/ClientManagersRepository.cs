using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Shared.Configurations;
using System;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.Managers
{
    public class ClientManagersRepository : IClientManagersRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public ClientManagersRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<Guid> SaveAsync(string token, string language, Guid? id, string firstName, string lastName, string email, string phoneNumber)
        {
            var requestModel = new ManagerRequestModel
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber
            };

            var apiRequest = new ApiRequest<ManagerRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.ClientUrl}{ApiConstants.Client.ManagersApiEndpoint}"
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<ManagerRequestModel>, ManagerRequestModel, BaseResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Id != null)
            {
                return response.Data.Id.Value;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }
    }
}
