using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Identity.Api.Areas.Accounts.ApiRequestModels;
using Identity.Api.Areas.Accounts.Models;
using Identity.Api.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Repositories
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

        public async Task<User> GetUserAsync(Guid? id, string token, string language)
        {
            var requestModel = new GetUserRequestModel
            {
                Id = id.ToString(),
            };

            var apiRequest = new ApiRequest<GetUserRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.IdentityUrl}{ApiConstants.Identity.UsersApiEndpoint}/{id}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<GetUserRequestModel>, GetUserRequestModel, User>(apiRequest);

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

        public async Task<Guid> SaveAsync(string id, string password, string firstName, string lastName, string token, string language)
        {
            var apiRequest = new ApiRequest<SaveUserRequestModel>
            {
                Language = language,
                Data = new SaveUserRequestModel
                {
                    Id = id,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName
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

        public async Task<Guid> SetPassword(Guid? expirationId, string password, string token, string language)
        {
            var requestModel = new SetUserPasswordRequestModel
            {
                ExpirationId = expirationId,
                Password = password
            };

            var apiRequest = new ApiRequest<SetUserPasswordRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.IdentityUrl}{ApiConstants.Identity.UsersSetPasswordApiEndpoint}"
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<SetUserPasswordRequestModel>, SetUserPasswordRequestModel, BaseResponseModel>(apiRequest);
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
