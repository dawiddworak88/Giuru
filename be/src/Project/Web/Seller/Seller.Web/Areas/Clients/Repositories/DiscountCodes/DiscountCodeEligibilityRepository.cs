using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Pricing.Services;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.Configurations;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.DiscountCodes
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

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            // Any other failure fails the save: the Basket is neither priced with an unverified code
            // nor stripped of a code that may still be valid.
            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return response.Data?.Code;
        }
    }
}
