using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Pricing.Services;
using Microsoft.Extensions.Options;
using Buyer.Web.Shared.ApiRequestModels.Clients;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.DomainModels.Clients;
using System;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Clients
{
    public class DiscountCodeEligibilityRepository : IDiscountCodeEligibilityRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _settings;

        public DiscountCodeEligibilityRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            _apiClientService = apiClientService;
            _settings = settings;
        }

        public async Task<string> GetValidDiscountCodeAsync(string token, string language, Guid? clientId, string discountCode)
        {
            if (clientId.HasValue is false || string.IsNullOrWhiteSpace(discountCode))
            {
                return null;
            }

            var apiRequest = new ApiRequest<DiscountCodeEligibilityRequestModel>
            {
                Language = language,
                Data = new DiscountCodeEligibilityRequestModel
                {
                    ClientId = clientId,
                    Code = discountCode
                },
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.ClientUrl}{ApiConstants.Client.DiscountCodesApiEndpoint}/eligibility"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<DiscountCodeEligibilityRequestModel>, DiscountCodeEligibilityRequestModel, ValidDiscountCode>(apiRequest);

            // Client.Api answers "not valid" with no content, so every failure - a not found included,
            // which would mean a missing route - fails the save: the Basket is neither priced with an
            // unverified code nor stripped of a code that may still be valid.
            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return response.Data?.Code;
        }
    }
}
