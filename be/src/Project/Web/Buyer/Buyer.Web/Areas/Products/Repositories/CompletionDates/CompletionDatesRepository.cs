using Buyer.Web.Areas.Products.ApiResponseModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories.CompletionDates
{
    public class CompletionDatesRepository : ICompletionDatesRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _settings;

        public CompletionDatesRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            _apiClientService = apiClientService;
            _settings = settings;
        }

        public async Task<int> GetAsync(string token, string language, Guid transportId, Guid conditionId, Guid? zoneId, Guid? campaignId, DateTime currentDate)
        {
            string queryParameters = $"?transportId={transportId}&conditionId={conditionId}&zoneId={zoneId}&campaignId={campaignId}&currentDate={currentDate}";

            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                EndpointAddress = $"{_settings.Value.CompletionDatesUrl}{ApiConstants.Catalog.CompletionDatesEndpoint}{queryParameters}",
                AccessToken = token
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, CompletionDateResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode)
            {
                return response.Data.CompletionDate;
            }

            return 0;
        }
    }
}
