using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Extensions;
using Foundation.Pricing.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Prices
{
    public class ApplicableDiscountCodeService : IApplicableDiscountCodeService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDiscountCodeEligibilityRepository _discountCodeEligibilityRepository;
        private readonly IOptions<AppSettings> _options;

        public ApplicableDiscountCodeService(
            IHttpContextAccessor httpContextAccessor,
            IDiscountCodeEligibilityRepository discountCodeEligibilityRepository,
            IOptions<AppSettings> options)
        {
            _httpContextAccessor = httpContextAccessor;
            _discountCodeEligibilityRepository = discountCodeEligibilityRepository;
            _options = options;
        }

        public async Task<string> GetValidAsync(string token, string discountCode)
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

            return await _discountCodeEligibilityRepository.GetValidDiscountCodeAsync(token, CultureInfo.CurrentUICulture.Name, clientId, discountCode);
        }
    }
}
