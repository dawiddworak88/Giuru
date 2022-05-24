using Buyer.Web.Areas.Dashboard.DomainModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Dashboard.Repositories.Identity
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

        public async Task<Guid> CreateAppSecretAsync(string token, string language)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.IdentityUrl}{ApiConstants.Identity.SecretsApiEndpoint}"
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return response.Data.Id.Value;
            }

            return default;
        }

        public async Task<Guid> GetSecretAsync(string token, string language)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.IdentityUrl}{ApiConstants.Identity.SecretsApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return response.Data.Id.Value;
            }

            return default;
        }
    }
}
