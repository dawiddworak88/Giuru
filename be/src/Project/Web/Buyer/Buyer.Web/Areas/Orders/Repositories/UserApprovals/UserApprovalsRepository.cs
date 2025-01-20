using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.Repositories.UserApprovals
{
    public class UserApprovalsRepository : IUserApprovalsRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _options;

        public UserApprovalsRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> options)
        {
            _apiClientService = apiClientService;
            _options = options;
        }

        public async Task<IEnumerable<UserApproval>> GetAsync(string token, string language, Guid id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_options.Value.IdentityUrl}{ApiConstants.Identity.UserApprovalsEndpoint}/{id}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, IEnumerable<UserApproval>>(apiRequest);

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
