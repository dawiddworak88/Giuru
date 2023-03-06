using Buyer.Web.Areas.Dashboard.ApiRequestModels;
using Buyer.Web.Areas.Dashboard.DomainModels;
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

namespace Buyer.Web.Areas.Dashboard.Repositories
{
    public class SalesAnalyticsRepository : ISalesAnalyticsRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _settings;

        public SalesAnalyticsRepository(
            IApiClientService apiClientService, 
            IOptions<AppSettings> settings)
        {
            _apiClientService = apiClientService;
            _settings = settings;
        }

        public async Task<IEnumerable<AnnualSalesItem>> GetAnnualSales(string token, string language, DateTime fromDate, DateTime toDate)
        {
            var requestModel = new SalesAnalyticsRequestModel
            {
                FromDate = fromDate,
                ToDate = toDate,
            };

            var apiRequest = new ApiRequest<SalesAnalyticsRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.AnalyticsUrl}{ApiConstants.Analytics.SalesAnalyticsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<SalesAnalyticsRequestModel>, SalesAnalyticsRequestModel, IEnumerable<AnnualSalesItem>>(apiRequest);

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
    }
}
