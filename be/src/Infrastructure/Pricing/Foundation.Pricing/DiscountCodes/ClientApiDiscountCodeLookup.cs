using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Pricing.ApiRequestModels;
using Foundation.Pricing.ApiResponseModels;
using Foundation.Pricing.Configurations;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Foundation.Pricing.DiscountCodes
{
    /// <summary>
    /// The one port to Client.Api, shared by the buyer and seller apps: one call with the caller's own token and an
    /// explicit client id. Client.Api checks that client against the organisation of the token, so a buyer can only ask
    /// for their own client. A failed call throws and is turned into "unverified" by <see cref="DiscountCodeValidator"/>;
    /// it is never read as an answer.
    /// </summary>
    public sealed class ClientApiDiscountCodeLookup : IDiscountCodeLookup
    {
        private readonly IApiClientService _apiClientService;
        private readonly IPricingSettings _pricingSettings;

        public ClientApiDiscountCodeLookup(
            IApiClientService apiClientService,
            IPricingSettings pricingSettings)
        {
            _apiClientService = apiClientService;
            _pricingSettings = pricingSettings;
        }

        public async Task<DiscountCodeValidation> ValidateAsync(Guid clientId, string discountCode, string token)
        {
            // The API client service encodes the query values.
            var apiRequest = new ApiRequest<DiscountCodeValidationApiRequestModel>
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Data = new DiscountCodeValidationApiRequestModel
                {
                    Code = discountCode,
                    ClientId = clientId
                },
                AccessToken = token,
                EndpointAddress = $"{_pricingSettings.ClientUrl}{ApiConstants.Client.DiscountCodesValidationApiEndpoint}"
            };

            var response = await _apiClientService
                .GetAsync<ApiRequest<DiscountCodeValidationApiRequestModel>, DiscountCodeValidationApiRequestModel, DiscountCodeValidationApiResponseModel>(apiRequest);

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
