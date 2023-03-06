using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Dashboard.ApiRequestModels;
using Seller.Web.Areas.Dashboard.DomainModels;
using Seller.Web.Shared.Configurations;
using System;
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

        public async Task<IEnumerable<CountrySalesItem>> GetCountriesSales(string token, string language, DateTime fromDate, DateTime toDate)
        {
            var requestModel = new SalesAnalyticsRequestModel
            {
                FromDate = fromDate,
                ToDate = toDate
            };

            var apiRequest = new ApiRequest<SalesAnalyticsRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.AnalyticsUrl}{ApiConstants.Analytics.CountriesSalesAnalyticsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<SalesAnalyticsRequestModel>, SalesAnalyticsRequestModel, IEnumerable<CountrySalesItem>>(apiRequest);

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

        public async Task<IEnumerable<DailySalesItem>> GetDailySales(string token, string language, DateTime fromDate, DateTime toDate)
        {
            var requestModel = new SalesAnalyticsRequestModel
            {
                FromDate = fromDate,
                ToDate = toDate
            };

            var apiRequest = new ApiRequest<SalesAnalyticsRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.AnalyticsUrl}{ApiConstants.Analytics.DailySalesAnalyticsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<SalesAnalyticsRequestModel>, SalesAnalyticsRequestModel, IEnumerable<DailySalesItem>>(apiRequest);

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

        public async Task<IEnumerable<ProductSalesApiItem>> GetProductsSales(string token, string language, DateTime fromDate, DateTime toDate, int? size, string orderBy)
        {
            var requestModel = new ProductsSalesAnalyticsApiRequestModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                OrderBy = orderBy,
                Size = size
            };

            var apiRequest = new ApiRequest<ProductsSalesAnalyticsApiRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.AnalyticsUrl}{ApiConstants.Analytics.ProductsSalesAnalyticsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<ProductsSalesAnalyticsApiRequestModel>, ProductsSalesAnalyticsApiRequestModel, IEnumerable<ProductSalesApiItem>>(apiRequest);

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
