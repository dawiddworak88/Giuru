using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Options;
using Seller.Web.Shared.ApiRequestModels;
using Seller.Web.Shared.Configurations;
using System;
using System.Threading.Tasks;
using System.Web;

namespace Seller.Web.Shared.Repositories.Identity
{
    public class IdentityRepository : IIdentityRepository
    {

        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public IdentityRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<Guid> SaveAsync(string token, string language, string name, string email, string communicationLanguage, string returnUrl)
        {
            var apiRequest = new ApiRequest<SaveUserRequestModel>
            {
                Language = language,
                Data = new SaveUserRequestModel
                {
                    Email = email,
                    Name = name,
                    CommunicationLanguage = communicationLanguage,
                    ReturnUrl = returnUrl
                },
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.IdentityUrl}{ApiConstants.Identity.UsersApiEndpoint}"
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<SaveUserRequestModel>, SaveUserRequestModel, BaseResponseModel>(apiRequest); 
            if (!response.IsSuccessStatusCode || response?.Data?.Id == null)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode)
            {
                return response.Data.Id.Value;
            }

            return default;
        }
    }
}
