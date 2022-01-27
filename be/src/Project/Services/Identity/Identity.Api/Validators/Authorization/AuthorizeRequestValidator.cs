using Foundation.Account.Definitions;
using Foundation.Localization;
using Identity.Api.Configurations;
using IdentityServer4.Validation;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Validators.Authorization
{
    public class AuthorizeRequestValidator : ICustomAuthorizeRequestValidator
    {
        private readonly IOptions<AppSettings> options;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public AuthorizeRequestValidator(
            IOptions<AppSettings> options,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.options = options;
            this.globalLocalizer = globalLocalizer;
        }

        public async Task ValidateAsync(CustomAuthorizeRequestValidationContext context)
        {
            var isSellerClaim = context.Result.ValidatedRequest.Subject.Claims.FirstOrDefault(x => x.Type == AccountConstants.IsSellerClaim);

            if (context.Result.ValidatedRequest.Subject.Identity.IsAuthenticated && context.Result.ValidatedRequest.ClientId == this.options.Value.SellerClientId.ToString() && (string.IsNullOrWhiteSpace(isSellerClaim?.Value) || isSellerClaim.Value != "true"))
            {
                context.Result.IsError = true;
                context.Result.Error = this.globalLocalizer.GetString("Unauthorized");
                context.Result.ErrorDescription = this.globalLocalizer.GetString("ErrorOccurred");
            }
        }
    }
}
