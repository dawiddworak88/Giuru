using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Shared.ApiRequestModels;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.DomainModels.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task AssignRolesAsync(string token, string language, string email, IEnumerable<string> roles)
        {
            var requestModel = new RolesRequestModel
            {
                Email = email,
                Roles = roles
            };

            var apiRequest = new ApiRequest<RolesRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.IdentityUrl}{ApiConstants.Identity.RolesApiEndpoint}"
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<RolesRequestModel>, RolesRequestModel, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode && response?.Data != null)
            {
                throw new CustomException(response.Data.Message, (int)response.StatusCode);
            }
        }

        public async Task<User> GetAsync(string token, string language, string email)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.IdentityUrl}{ApiConstants.Identity.UsersApiEndpoint}/{email}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, User>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data is not null)
            {
                return response.Data;
            }

            return default;
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

        public async Task<Guid> UpdateAsync(string token, string language, Guid? id, string email, string name, string communicationLanguage)
        {
            var requestModel = new UpdateClientRequestModel
            {
                Id = id,
                Email = email,
                Name = name,
                CommunicationLanguage = communicationLanguage
            };

            var apiRequest = new ApiRequest<UpdateClientRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.IdentityUrl}{ApiConstants.Identity.UsersApiEndpoint}"
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<UpdateClientRequestModel>, UpdateClientRequestModel, BaseResponseModel>(apiRequest);
            if (response.IsSuccessStatusCode)
            {
                return response.Data.Id.Value;
            }

            return default;
        }
    }
}
