using Buyer.Web.Areas.Dashboard.DomainModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Dashboard.Repositories
{
    public class SalesAnalyticsRepository : ISalesAnalyticsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public SalesAnalyticsRepository(
            IApiClientService apiClientService, 
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<IEnumerable<AnnualSalesItem>> GetAnnualSales(string token, string language)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.AnalyticsUrl}{ApiConstants.Analytics.SalesAnalyticsApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, IEnumerable<AnnualSalesItem>>(apiRequest);

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

        public async Task<IEnumerable<Product>> GetProductsSales(string token, string language)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.AnalyticsUrl}{ApiConstants.Analytics.ProductsSalesAnalyticsApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, IEnumerable<Product>>(apiRequest);

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
