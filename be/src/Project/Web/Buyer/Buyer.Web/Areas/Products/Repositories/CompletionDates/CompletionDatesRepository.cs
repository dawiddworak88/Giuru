using Buyer.Web.Areas.Products.ApiRequestModels;
using Buyer.Web.Areas.Products.ApiResponseModels;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.DomainModels.Clients;
using Buyer.Web.Areas.Products.DomainModels;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Product>> PostAsync(string token, string language, IEnumerable<Product> products, List<ClientFieldValue> clientFields, DateTime currentDate)
        {
            var requestModel = new CompletionDateRequestModel
            {
                Products = products,
                ClientFields = clientFields,
                CurrentDate = currentDate,
                Language = language,
            };

            var apiRequest = new ApiRequest<CompletionDateRequestModel>
            {
                Language = language,
                Data = requestModel,
                EndpointAddress = $"{_settings.Value.CompletionDatesUrl}{ApiConstants.Catalog.CompletionDatesEndpoint}",
                AccessToken = token
            };

            var response = await _apiClientService.PostAsync<ApiRequest<CompletionDateRequestModel>, CompletionDateRequestModel, CompletionDateResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode)
            {
                return response.Data.Products; 
            }

            return null;
        }
    }
}
