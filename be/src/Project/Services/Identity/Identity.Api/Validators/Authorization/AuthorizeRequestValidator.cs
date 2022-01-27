using Foundation.Account.Definitions;
using IdentityServer4.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Validators.Authorization
{
    public class AuthorizeRequestValidator : ICustomAuthorizeRequestValidator
    {
        public async Task ValidateAsync(CustomAuthorizeRequestValidationContext context)
        {
            var isSellerClaim = context.Result.ValidatedRequest.Subject.Claims.FirstOrDefault(x => x.Type == AccountConstants.IsSellerClaim);

            if (context.Result.ValidatedRequest.Subject.Identity.IsAuthenticated && context.Result.ValidatedRequest.ClientId == "663bba90-0036-4a58-8516-39faa8baba87" && (string.IsNullOrWhiteSpace(isSellerClaim?.Value) || isSellerClaim.Value != "true"))
            {
                context.Result.IsError = true;
                context.Result.Error = "Unauthorized";
                context.Result.ErrorDescription = "An error has occurred.";
            }
        }
    }
}
