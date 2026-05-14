using Buyer.Web.Shared.ApiRequestModels.LeadTime;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.DomainModels.LeadTime;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<LeadTimeItem>> GetLeadTimesAsync(string accessToken, string[] skus)
        {
            if (skus == null || skus.Length == 0)
            {
                return Array.Empty<LeadTimeItem>();
            }

            int total = (int)Math.Ceiling(skus.Length / (double)Constants.MaxItemsPerPage);

            var leadTimeResults = new List<LeadTimeItem>();

            int pageIndex = Constants.DefaultPageIndex;

            for (int i = 0; i < total; i++)
            {
                var pagedSkus = skus.Skip(i * Constants.MaxItemsPerPage).Take(Constants.MaxItemsPerPage).ToEndpointParameterString();

                var requestModel = new GetLeadTimeBySkusRequestModel
                {
                    Skus = pagedSkus,
                    PageIndex = pageIndex,
                };

                var apiRequest = new ApiRequest<GetLeadTimeBySkusRequestModel>
                {
                    AccessToken = accessToken,
                    Data = requestModel,
                    EndpointAddress = $"{_options.Value.LeadTimeUrl}{ApiConstants.LeadTime.LeadTimeBySkusApiEndpoint}"
                };

                var response = await _apiClientService.GetAsync<ApiRequest<GetLeadTimeBySkusRequestModel>, GetLeadTimeBySkusRequestModel, PagedLeadTimeResults>(apiRequest);

                if (response == null || !response.IsSuccessStatusCode || response.Data?.Items is null)
                {
                    _logger.LogError(
                        "Failed to retrieve lead times for SKUs: {Skus}. " +
                        "Status Code: {StatusCode}, " +
                        "PageIndex: {PageIndex}, " +
                        "Message: {Message}",
                        requestModel.Skus,
                        response?.StatusCode,
                        requestModel.PageIndex,
                        response?.Message);

                    pageIndex++;
                    continue;
                }

                leadTimeResults.AddRange(response.Data.Items);

                pageIndex++;
            }

            return leadTimeResults;
        }
    }
}
