using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.DomainModels.User;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using IdentityServer4.Validation;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Identity
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _options;

        public IdentityRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> options)
        {
            _apiClientService = apiClientService;
            _options = options;
        }

        public async Task<User> GetAsync(string token, string language, string email)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_options.Value.IdentityUrl}{ApiConstants.Identity.UsersApiEndpoint}/{email}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, User>(apiRequest);

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data is not null)
            {
                return response.Data;
            }

            return default;
        }
    }
}
