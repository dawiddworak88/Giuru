using Buyer.Web.Shared.ApiRequestModels.LeadTime;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.DomainModels.LeadTime;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.LeadTime
{
    public class LeadTimeRepository : ILeadTimeRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _options;
        private readonly ILogger<LeadTimeRepository> _logger;

        public LeadTimeRepository(
            IApiClientService apiClientService, 
            IOptions<AppSettings> options,
            ILogger<LeadTimeRepository> logger)
        {
            _apiClientService = apiClientService;
            _options = options;
            _logger = logger;
        }

        public async Task<PagedLeadTimeResults> GetLeadTimesAsync(string accessToken, string[] skus)
        {
            var requestModel = new GetLeadTimeBySkusRequestModel
            {
                Skus = skus.ToEndpointParameterString()
            };

            var apiRequest = new ApiRequest<GetLeadTimeBySkusRequestModel>
            {
                AccessToken = accessToken,
                Data = requestModel,
                EndpointAddress = $"{_options.Value.LeadTimeUrl}{ApiConstants.LeadTime.LeadTimeBySkusApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<GetLeadTimeBySkusRequestModel>, GetLeadTimeBySkusRequestModel, PagedLeadTimeResults>(apiRequest);

            if (!response.IsSuccessStatusCode || response.Data?.Items is null)
            {
                _logger.LogError(
                    "Failed to retrieve lead times for SKUs: {Skus}. " +
                    "Status Code: {StatusCode}, " +
                    "Message: {Message}", 
                    requestModel.Skus, 
                    response.StatusCode, 
                    response.Message);

                return default;
            }

            return response.Data;
        }
    }
}
