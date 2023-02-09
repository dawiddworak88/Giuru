using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Dashboard.DomainModels;
using Seller.Web.Shared.Configurations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Dashboard.Repositories
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

        public async Task<IEnumerable<CountrySalesItem>> GetCountriesSales(string token, string language)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.AnalyticsUrl}{ApiConstants.Analytics.CountriesSalesAnalyticsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, IEnumerable<CountrySalesItem>>(apiRequest);

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

        public async Task<IEnumerable<DailySalesItem>> GetDailySales(string token, string language)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.AnalyticsUrl}{ApiConstants.Analytics.DailySalesAnalyticsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, IEnumerable<DailySalesItem>>(apiRequest);

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
