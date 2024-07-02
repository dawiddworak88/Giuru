using Buyer.Web.Areas.Products.ApiRequestModels;
using Buyer.Web.Areas.Products.ApiResponseModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
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
            var requestModel = new CompletionDateRequestModel
            {
                TransportId = transportId,
                ConditionId = conditionId,
                ZoneId = zoneId,
                CampaignId = campaignId,
                CurrentDate = currentDate
            };

            var apiRequest = new ApiRequest<CompletionDateRequestModel>
            {
                Language = language,
                Data = requestModel,
                EndpointAddress = $"{_settings.Value.CompletionDatesUrl}{ApiConstants.Catalog.CompletionDatesEndpoint}",
                AccessToken = token
            };

            var response = await _apiClientService.GetAsync<ApiRequest<CompletionDateRequestModel>, CompletionDateRequestModel, CompletionDateResponseModel>(apiRequest);

            Console.WriteLine(JsonSerializer.Serialize(response));

            if (response.IsSuccessStatusCode)
            {
                return response.Data.CompletionDate;
            }

            return 0;
        }
    }
}
