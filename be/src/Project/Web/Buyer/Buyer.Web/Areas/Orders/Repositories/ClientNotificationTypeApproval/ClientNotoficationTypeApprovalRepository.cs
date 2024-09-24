using Buyer.Web.Areas.Orders.Repositories.NotificationTypeApproval;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.ApiExtensions.Services.ApiClientServices;
using System.Collections.Generic;
using Foundation.Extensions.Exceptions;

namespace Buyer.Web.Areas.Orders.Repositories.ClientNotificationTypeApproval
{
    public class ClientNotoficationTypeApprovalRepository : IClientNotificationTypeApproval
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly IApiClientService _apiClientService;

        public ClientNotoficationTypeApprovalRepository(
            IOptions<AppSettings> settings,
            IApiClientService apiClientService)
        {
            _settings = settings;
            _apiClientService = apiClientService;
        }

        public async Task<IEnumerable<DomainModels.ClientNotificationTypeApproval>> GetAsync(string token, string language, Guid? clientId)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.NotificationTypesApiApprovalEndpoint}/{clientId}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, IEnumerable<DomainModels.ClientNotificationTypeApproval>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data is not null)
            {
                return response.Data;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }
    }
}
