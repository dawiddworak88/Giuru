using Foundation.Account.Definitions;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Services.ProfileServices
{
    public class ProfileService : IProfileService
    {
        public ProfileService()
        {
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject?.GetSubjectId();

            if (!string.IsNullOrWhiteSpace(sub))
            {
                var user = await this.userService.FindByIdAsync(sub);

                if (user != null && user.Seller != null && user.EmailConfirmed && user.Seller.IsActive)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(AccountConstants.SellerIdClaim, user.Seller.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.Seller.Name)
                    };

                    context.IssuedClaims.AddRange(claims);
                }
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject?.GetSubjectId();

            if (!string.IsNullOrWhiteSpace(sub))
            {
                var user = await this.userService.FindByIdAsync(sub);

                if (user != null && user.Seller != null && user.EmailConfirmed && user.Seller.IsActive)
                {
                    context.IsActive = true;
                    return;
                }
            }

            context.IsActive = false;
        }
    }
}
