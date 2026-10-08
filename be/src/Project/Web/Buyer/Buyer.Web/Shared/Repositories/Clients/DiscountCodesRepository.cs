using Buyer.Web.Shared.ApiRequestModels;
using Buyer.Web.Shared.ApiResponseModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Pricing.DiscountCodes;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Clients
{
    public class DiscountCodesRepository : IDiscountCodesRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _settings;

        public DiscountCodesRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            _apiClientService = apiClientService;
            _settings = settings;
        }

        public async Task<DiscountCodeValidation> ValidateAsync(string token, string language, Guid clientId, string code, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // The API client service encodes the query values.
            var apiRequest = new ApiRequest<DiscountCodeValidationApiRequestModel>
            {
                Language = language,
                Data = new DiscountCodeValidationApiRequestModel
                {
                    Code = code,
                    ClientId = clientId
                },
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.DiscountCodesValidationApiEndpoint}"
            };

            var response = await _apiClientService
                .GetAsync<ApiRequest<DiscountCodeValidationApiRequestModel>, DiscountCodeValidationApiRequestModel, DiscountCodeValidationApiResponseModel>(apiRequest)
                .WaitAsync(cancellationToken);

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            // An empty body is not an answer either.
            return response.Data is null
                ? DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Unavailable)
                : DiscountCodeValidation.FromApiResponse(response.Data.Status, response.Data.DiscountCode);
        }
    }
}
