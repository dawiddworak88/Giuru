using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Extensions;
using Foundation.Extensions.Exceptions;
using Foundation.Pricing.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Prices
{
    public class ApplicableDiscountCodeService : IApplicableDiscountCodeService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDiscountCodeEligibilityRepository _discountCodeEligibilityRepository;
        private readonly IOptions<AppSettings> _options;
        private readonly ILogger<ApplicableDiscountCodeService> _logger;

        public ApplicableDiscountCodeService(
            IHttpContextAccessor httpContextAccessor,
            IDiscountCodeEligibilityRepository discountCodeEligibilityRepository,
            IOptions<AppSettings> options,
            ILogger<ApplicableDiscountCodeService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _discountCodeEligibilityRepository = discountCodeEligibilityRepository;
            _options = options;
            _logger = logger;
        }

        public async Task<string> GetApplicableAsync(string token, string discountCode)
        {
            if (_options.Value.IsGrulaConfigured is false || string.IsNullOrWhiteSpace(discountCode))
            {
                return null;
            }

            var clientId = _httpContextAccessor.HttpContext?.User.GetClientId();

            if (clientId.HasValue is false)
            {
                return null;
            }

            try
            {
                return await _discountCodeEligibilityRepository.GetValidDiscountCodeAsync(token, CultureInfo.CurrentUICulture.Name, clientId, discountCode);
            }
            catch (Exception exception) when (exception is CustomException or HttpRequestException or TaskCanceledException)
            {
                _logger.LogWarning(exception, "Couldn't check the Discount Code for Client {ClientId}; continuing without it.", clientId);
                return null;
            }
        }
    }
}
